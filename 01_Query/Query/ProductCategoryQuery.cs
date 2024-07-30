using _01_Framework.Application;
using _01_Framework.Application.Pagination;
using _01_Query.Contract.Product;
using _01_Query.Contract.ProductCategory;
using DiscountManagement.Infrastructure.EfCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_Query.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        public ProductCategoryQuery(ShopContext shopContext, InventoryContext inventoryContext, DiscountContext discountContext)
        {
            _shopContext = shopContext;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProdcutCategoryQueryModel> GetProductCategories()
        {
            return _shopContext.ProductCategories.Select(x => new ProdcutCategoryQueryModel
            {
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug
            }).AsNoTracking().ToList();
        }

        public List<ProdcutCategoryQueryModel> GetProductCategoriesWithProducts()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate }).AsNoTracking().ToList();

            var categories = _shopContext.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category).Select(x => new ProdcutCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                Products = x.Products != null ? MapProducts(x.Products) : new()
            }).AsNoTracking().ToList();

            foreach (var category in categories)
            {
                foreach (var product in category.Products)
                {
                    var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                    if (productInventory != null)
                    {
                        var price = productInventory.UnitPrice;

                        product.Price = price.ToMoney();

                        var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                        if (discount != null)
                        {
                            var discountRate = discount.DiscountRate;
                            product.DiscountRate = discountRate;
                            product.HasDiscount = discountRate > 0;
                            var discountAmount = Math.Round((price * discountRate) / 100);
                            product.PriceWithDiscount = (price - discountAmount).ToMoney();
                        }
                    }
                }
            }
            return categories;
        }

        public ProdcutCategoryQueryModel GetProductCategoryWithProductsBy(string slug, PaginationOptions paginationOptions, FilteringOptions<ProdcutCategoryQueryModel> filteringOptions, SortingOptions<ProdcutCategoryQueryModel> sortingOptions)
        {
            var categoryIsChild = _shopContext
                .ProductCategories
                .Where(x => x.Slug == slug && x.ParentId != null)
                .Any();

            return categoryIsChild ? GetProductCategoryChildWithProducts(slug, paginationOptions, filteringOptions, sortingOptions) : GetProductCategoryParentWithAllChildProducts(slug, paginationOptions, filteringOptions, sortingOptions);

        }

        public List<ProductCategoryWithChildren> GetCategoryWithChildren()
        {
            return _shopContext.
                 ProductCategories.
                 Where(x => x.ParentId == null).
                 Select(x => new ProductCategoryWithChildren
                 {
                     Slug = x.Slug,
                     Name = x.Name,
                     Children = ProductCategoryChildrenMapper(x.Children)
                 }).
                 ToList();
        }

        public List<ProdcutCategoryQueryModel> GetCategryWithProudctCount()
        {
            return _shopContext
                .ProductCategories
                .Where(x => x.ParentId != null)
                .Include(x => x.Products)
                .Select(x => new ProdcutCategoryQueryModel
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    ProductCount = x.Products.Count.ToString()
                }
                )
                .AsNoTracking()
                .ToList();
        }

        private static List<ProductCategoryWithChildren> ProductCategoryChildrenMapper(List<ProductCategory> children)
        {
            List<ProductCategoryWithChildren> withChildrens = new();

            children.ForEach(x =>
            withChildrens.Add(new()
            {
                Name = x.Name,
                Slug = x.Slug
            }));

            return withChildrens;
        }

        public List<ProductCategoryWithChildren> GetCategoryAndPictureWithChildren()
        {
            return _shopContext.
                 ProductCategories.
                 Where(x => x.ParentId == null).
                 Select(x => new ProductCategoryWithChildren
                 {
                     Slug = x.Slug,
                     Name = x.Name,
                     Picture = x.Picture,
                     PictureAlt = x.PictureAlt,
                     PictureTitle = x.PictureTitle,
                     Children = ProductCategoryChildrenMapper(x.Children)
                 }).
                 ToList();
        }

        private static List<ProductQueryModel> MapProducts(List<Product> products)
        {
            return products.Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category.Name,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Slug = x.Slug,
                CategorySlug = x.Category.Slug
            }).ToList();
        }

        private ProdcutCategoryQueryModel GetProductCategoryParentWithAllChildProducts(string slug, PaginationOptions paginationOptions, FilteringOptions<ProdcutCategoryQueryModel> filteringOptions, SortingOptions<ProdcutCategoryQueryModel> sortingOptions)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).AsNoTracking().ToList();



            var category = _shopContext
                .ProductCategories
                .Where(x => x.Slug == slug)
                .Include(x => x.Children)
                .ThenInclude(x =>
                x.Products
                .Skip(paginationOptions.PageSize * (paginationOptions.PageNumber - 1))
                .Take(paginationOptions.PageSize))
                .DefaultIfEmpty()
                .Select(x => new ProdcutCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    Description = x.Description,
                    MetaDescription = x.MetaDescription,
                    KeyWords = x.KeyWords,
                    ProductCount = x.Products.Count.ToString(),
                    ParentName = x.Parent == null ? "" : x.Parent.Name,
                    Products = MapChildsProduct(x.Children)
                })
                .AsNoTracking();

            if (filteringOptions != null)
            {
                category
               .Where(filteringOptions.FilteringWhere);

                if (sortingOptions != null)
                {
                    category
               .OrderBy(sortingOptions.SortingWhere);
                }
            }

            var result = category
                .SingleOrDefault();

            foreach (var product in result.Products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;

                    product.Price = price.ToMoney();

                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return result;
        }

        private static List<ProductQueryModel> MapChildsProduct(List<ProductCategory> productCategories)
        {
            List<ProductQueryModel> products = new();

            foreach (var category in productCategories)
            {
                foreach (var product in category.Products)
                {
                    products.Add(new ProductQueryModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Category = product.Category.Name,
                        Picture = product.Picture,
                        PictureTitle = product.PictureTitle,
                        PictureAlt = product.PictureAlt,
                        Slug = product.Slug,
                        CategorySlug = product.Category.Slug
                    });
                }
            }

            return products;
        }

        private ProdcutCategoryQueryModel GetProductCategoryChildWithProducts(string slug, PaginationOptions paginationOptions, FilteringOptions<ProdcutCategoryQueryModel> filteringOptions, SortingOptions<ProdcutCategoryQueryModel> sortingOptions)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).AsNoTracking().ToList();

            var cadtegory = _shopContext
               .ProductCategories
               .Where(x => x.Slug == slug)
               .Include(x => x.Products)
               .DefaultIfEmpty()
               .AsNoTracking()
               .SingleOrDefault();

            var category = _shopContext
                .ProductCategories
                .Where(x => x.Slug == slug)
                .Include(x => x.Products)
                .DefaultIfEmpty()
                .Select(x => new ProdcutCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                    Description = x.Description,
                    MetaDescription = x.MetaDescription,
                    KeyWords = x.KeyWords,
                    ParentName = x.Parent == null ? "" : x.Parent.Name,
                    Products = MapProducts(x.Products)
                })
                .AsNoTracking();

            if (paginationOptions != null)
            {
                category
               .Skip(paginationOptions.PageSize - (paginationOptions.PageNumber - 1))
               .Take(paginationOptions.PageNumber);
            }
            else if (filteringOptions != null)
            {
                category
               .Where(filteringOptions.FilteringWhere);

                if (sortingOptions != null)
                {
                    category
               .OrderBy(sortingOptions.SortingWhere);
                }
            }

            var result = category
                .SingleOrDefault();

            foreach (var product in result.Products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;

                    product.Price = price.ToMoney();

                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return result;
        }
    }
}
