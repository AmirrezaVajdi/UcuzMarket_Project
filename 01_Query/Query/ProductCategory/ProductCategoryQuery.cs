using _01_Framework.Application;
using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
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

            var discounts = _discountContext.CustomerDiscounts
                           .Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date)
                           .Select(x => new { x.ProductId, x.DiscountRate }).AsNoTracking().ToList();

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
        public ProdcutCategoryQueryModel GetProductCategoryWithProductsBy(string slug, PaginationOptions paginationOptions)
        {
            var categoryIsChild = _shopContext
                .ProductCategories
                .Where(x => x.Slug == slug && x.ParentId != null)
                .Any();

            return categoryIsChild ? GetProductCategoryChildWithProducts(slug, paginationOptions) : GetProductCategoryParentWithAllChildProducts(slug, paginationOptions);

        }
        private ProdcutCategoryQueryModel GetProductCategoryParentWithAllChildProducts(string slug, PaginationOptions paginationOptions)
        {
            var categoryQuery = _shopContext
                .ProductCategories
                .Where(x => x.Slug == slug)
                .Include(x => x.Children)
                .ThenInclude(x => x.Products
                .Skip(paginationOptions.PageSize * (paginationOptions.PageNumber - 1))
                .Take(paginationOptions.PageNumber == 1 ? paginationOptions.PageSize - 1 : paginationOptions.PageSize))
                .DefaultIfEmpty()
                .AsNoTracking();

            var selectedData = categoryQuery
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
             });

            var category = selectedData
                .SingleOrDefault();


            var ProductsId = GetProductsId(category.Products);


            var discounts = _discountContext
                .CustomerDiscounts
                .Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date)
                .Where(x => ProductsId.Any(z => x.ProductId == z))
                .Select(x => new
                {
                    x.ProductId,
                    x.DiscountRate,
                    x.EndDate

                })
                .AsNoTracking()
                .ToList();


            var inventories = _inventoryContext
                .Inventory
                .Where(x => ProductsId.Any(z => x.ProductId == z))
               .Select(x => new
               {
                   x.ProductId,
                   x.UnitPrice,
                   x.InStock
               })
               .AsNoTracking();

            var result = selectedData
                .SingleOrDefault();


            foreach (var product in result.Products)
            {
                var productInventory = inventories
                    .SingleOrDefault(x => x.ProductId == product.Id);

                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.IsInStock = productInventory.InStock;


                    product.Price = price.ToMoney();

                    var discount = discounts.SingleOrDefault(x => x.ProductId == product.Id);
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

            result.PageNumber = paginationOptions.PageNumber;

            result.ProductCount = GetParentProductsCount(slug).ToString();

            result.TotalPage = (long)Math.Ceiling(double.Parse(result.ProductCount) / paginationOptions.PageSize);

            return result;
        }
        private ProdcutCategoryQueryModel GetProductCategoryChildWithProducts(string slug, PaginationOptions paginationOptions)
        {
            var categories = _shopContext
                .ProductCategories
              .Where(x => x.Slug == slug)
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
                  ParentSlug = x.Parent == null ? "" : x.Parent.Slug
              })
              .SingleOrDefault();

            var products = _shopContext
                .Products
                .Where(x => x.CategoryId == categories.Id)
                .Skip(paginationOptions.PageSize * (paginationOptions.PageNumber - 1))
                .Take(paginationOptions.PageNumber == 1 ? paginationOptions.PageSize : paginationOptions.PageSize)
                .DefaultIfEmpty()
                .AsNoTracking()
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    Category = categories.Name,
                    CategorySlug = categories.Slug,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Code = x.Code,
                    Name = x.Name,
                })
                .ToList();

            categories.Products = products;

            var ProductsId = GetProductsId(products);

            var discounts = _discountContext
                .CustomerDiscounts
                .Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date)
                .Where(x => ProductsId.Any(z => x.ProductId == z))
                .Select(x => new
                {
                    x.ProductId,
                    x.DiscountRate,
                    x.EndDate
                })
                .AsNoTracking()
                .ToList();


            var inventories = _inventoryContext
                .Inventory
                .Where(x => ProductsId.Any(z => x.ProductId == z))
                .Select(x => new
                {
                    x.ProductId,
                    x.UnitPrice,
                    x.InStock
                })
               .AsNoTracking();

            foreach (var product in products)
            {
                var productInventory = inventories
                    .FirstOrDefault(x => x.ProductId == product.Id);

                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.IsInStock = productInventory.InStock;


                    product.Price = price.ToMoney();

                    var discount = discounts.SingleOrDefault(x => x.ProductId == product.Id);
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

            categories.PageNumber = paginationOptions.PageNumber;

            categories.ProductCount = GetProductCounts(slug).ToString();

            categories.TotalPage = (long)Math.Ceiling(double.Parse(categories.ProductCount) / paginationOptions.PageSize);

            return categories;
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
        private List<long> GetProductsId(List<ProductQueryModel> model)
        {
            List<long> ProductsId = new(model.Count);

            model.ForEach((x) => ProductsId.Add(x.Id));

            return ProductsId;
        }
        private long GetParentProductsCount(string slug)
        {
            long parentId = _shopContext
                .ProductCategories
                .Where(x => x.Slug == slug)
                .Select(x => x.Id)
                .SingleOrDefault();

            var subCategoriesId = _shopContext
                .ProductCategories
                .Where(x => x.ParentId == parentId)
                .Select(x => x.Id)
                .ToList();

            var products = _shopContext
                .Products
                .AsQueryable();

            products = products.Where(x => subCategoriesId.Any(z => x.CategoryId == z));

            return products
                  .Count();
        }
        private long GetProductCounts(string slug)
        {
            var categoryId = _shopContext.ProductCategories
                .Where(x => x.Slug == slug)
                .Select(x => x.Id)
                .SingleOrDefault();

            return _shopContext.Products
                        .Where(p => p.CategoryId == categoryId)
                        .Count();
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
    }
}
