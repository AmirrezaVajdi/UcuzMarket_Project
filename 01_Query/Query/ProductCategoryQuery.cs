using _01_Framework.Application;
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
                Products = MapProducts(x.Products)
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

        public ProdcutCategoryQueryModel GetProductCategoryWithProductsBy(string slug)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).AsNoTracking().ToList();

            var category = _shopContext.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category).Select(x => new ProdcutCategoryQueryModel
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
                Products = MapProducts(x.Products)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);


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
                        product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round((price * discountRate) / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return category;
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
    }
}
