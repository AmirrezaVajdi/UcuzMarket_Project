using _01_Framework.Application;
using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
using _01_Query.Contract.Comment;
using _01_Query.Contract.Product;
using _01_Query.Contract.ProductCategory;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagement.Infrastructure.EfCore;
using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Infrastructure.EFCore;

namespace _01_Query.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        private readonly CommentContext _commentContext;
        public ProductQuery(ShopContext shopContext, InventoryContext inventoryContext, DiscountContext discountContext, CommentContext commentContext)
        {
            _shopContext = shopContext;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
            _commentContext = commentContext;
        }

        public (List<ProductQueryModel>, PaginationResult) GetPopularProducts(PaginationOptions paginationOptions)
        {
            var productsQuery = _shopContext
                .Products
                .Skip(paginationOptions.PageSize * (paginationOptions.PageNumber - 1))
                .Take(paginationOptions.PageSize)
                .OrderByDescending(x => x.Id)
                .AsNoTracking();

            var selectedData = productsQuery
             .Select(x => new ProductQueryModel
             {
                 Name = x.Name,
                 Picture = x.Picture,
                 PictureAlt = x.PictureAlt,
                 PictureTitle = x.PictureTitle,
                 Slug = x.Slug,
             });

            var products = selectedData
                .ToList();


            var ProductsId = GetProductsId(products);


            var discounts = _discountContext
                .CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
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
                .ToList();


            foreach (var product in products)
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

            PaginationResult paginationResult = new();

            paginationResult.PageNumber = paginationOptions.PageNumber;

            paginationResult.ProductCount = GetProductsCount();

            paginationResult.TotalPage = (long)Math.Ceiling(double.Parse(paginationResult.ProductCount.ToString()) / paginationOptions.PageSize);

            return (result, paginationResult);
        }

        public ProductQueryModel GetProductDetails(string slug)
        {
            var inventory = _inventoryContext.Inventory
                .Select(x => new { x.ProductId, x.UnitPrice, x.InStock })
                .AsNoTracking()
                .ToList();

            var discounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
                .Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).
                AsNoTracking()
                .ToList();


            var product = _shopContext.Products
                .Include(x => x.Category)
                .Include(x => x.ProductPictures)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Category = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    ShortDescription = x.ShortDescription,
                    Code = x.Code,
                    Description = x.Description,
                    Keywords = x.KeyWords,
                    MetaDescription = x.MetaDescription,
                    Pictures = MapProductPictures(x.ProductPictures),
                }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

            if (product == null)
                return new ProductQueryModel();

            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (productInventory != null)
            {
                product.IsInStock = productInventory.InStock;
                var price = productInventory.UnitPrice;

                product.Price = price.ToMoney();
                product.DoublePrice = price;
                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                if (discount != null)
                {
                    var discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = discountRate > 0;
                    var discountAmount = Math.Round(price * discountRate / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }

            product.Comments = _commentContext.Comments
                .Where(x => x.Type == CommentType.Product)
                .Where(x => x.OwnerRecordId == product.Id)
                .Where(x => x.IsConfirmed)
                .Where(x => !x.IsCanceled)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Message = x.Message
                })
                .OrderByDescending(x => x.Id)
                .ToList();


            return product;
        }

        private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> productPictures)
        {
            return productPictures.Select(x => new ProductPictureQueryModel
            {
                IsRemoved = x.IsRemoved,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PicutreAlt = x.PicutreAlt,
                ProductId = x.ProductId
            }).Where(x => !x.IsRemoved).ToList();
        }

        public List<ProductQueryModel> Search(string value)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).AsNoTracking().ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).AsNoTracking().ToList();

            var query = _shopContext.Products.Include(x => x.Category).Select(x => new ProductQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category.Name,
                CategorySlug = x.Category.Slug,
                Picture = x.Picture,
                PictureTitle = x.PictureTitle,
                PictureAlt = x.PictureAlt,
                Slug = x.Slug,
                ShortDescription = x.ShortDescription
            }).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(value))
                query = query.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));

            var products = query.OrderByDescending(x => x.Id).ToList();

            foreach (var product in products)
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
                        var discountAmount = Math.Round(price * discountRate / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return products;
        }

        public List<CartItem> CheckInventoryStatus(List<CartItem> cartItems)
        {
            var inventory = _inventoryContext.Inventory.ToList();

            foreach (var cartItem in cartItems.Where(cartItem =>
                inventory.Any(x => x.ProductId == cartItem.Id && x.InStock)))
            {
                var itemInventory = inventory.Find(x => x.ProductId == cartItem.Id);
                cartItem.IsInStock = itemInventory.CalculateCurrentCount() >= cartItem.Count;
            }

            return cartItems;
        }

        public List<ProductQueryModel> GetDiscountedProducts(int take = 4)
        {

            var inventories = _inventoryContext
                .Inventory
                .Select(x => new
                {
                    x.ProductId,
                    x.UnitPrice,
                    x.InStock
                }
                )
                .AsNoTracking()
                .ToList();

            var discounts = _discountContext
               .CustomerDiscounts
               .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
               .Select(x => new
               {
                   x.ProductId,
                   x.DiscountRate
               }
               ).AsNoTracking()
               .ToList();

            var products = _shopContext
                .Products
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug
                })
                .OrderByDescending(x => x.Id)
                 .Take(take)
                 .AsNoTracking()
                 .ToList();

            var productsToRemove = new List<ProductQueryModel>();


            foreach (var product in products)
            {
                if (!discounts.Any(x => x.ProductId == product.Id))
                {
                    productsToRemove.Add(product);
                }
            }

            foreach (var productToRemove in productsToRemove)
            {
                products.Remove(productToRemove);
            }

            foreach (var product in products)
            {
                var productInventory = inventories.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.IsInStock = productInventory.InStock;

                    product.Price = price.ToMoney();

                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round(price * discountRate / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }

            return products;
        }
        private List<long> GetProductsId(List<ProductQueryModel> model)
        {
            List<long> ProductsId = new(model.Count);

            model.ForEach((x) => ProductsId.Add(x.Id));

            return ProductsId;
        }
        private long GetProductsCount()
        {
            return _shopContext.Products
                       .Count();
        }
    }
}
