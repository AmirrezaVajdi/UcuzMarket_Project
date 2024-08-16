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
                 Id = x.Id,
                 Name = x.Name,
                 Picture = x.Picture,
                 PictureAlt = x.PictureAlt,
                 PictureTitle = x.PictureTitle,
                 Slug = x.Slug
             });

            var products = selectedData
                .ToList();


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
                    .SingleOrDefault(x => x.ProductId == product.Id);

                double price = 0;

                if (productInventory != null)
                {
                    price = productInventory.UnitPrice;
                    product.IsInStock = productInventory.InStock;
                    product.Price = price.ToMoney();
                }

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

            PaginationResult paginationResult = new();

            paginationResult.PageNumber = paginationOptions.PageNumber;

            paginationResult.ProductCount = GetProductsCount();

            paginationResult.TotalPage = (long)Math.Ceiling(double.Parse(paginationResult.ProductCount.ToString()) / paginationOptions.PageSize);

            return (products, paginationResult);
        }

        public ProductQueryModel GetProductDetails(string slug)
        {
            var product = _shopContext
                .Products
                .Where(x => x.Slug == slug)
                .Include(x => x.Category)
                .Include(x => x.ProductPictures)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Category = x.Category.Name,
                    CategorySlug = x.Category.Slug,
                    Code = x.Code,
                    Description = x.Description,
                    Keywords = x.KeyWords,
                    MetaDescription = x.MetaDescription,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    Pictures = MapProductPictures(x.ProductPictures.Where(x => !x.IsRemoved).ToList())
                })
                .AsNoTracking()
                .SingleOrDefault();

            product.Comments = _commentContext
                .Comments
                .OrderByDescending(x => x.CreationDate)
                .Include(x => x.Child)
                .DefaultIfEmpty()
                .Where(x => x.Type == CommentType.Product && x.OwnerRecordId == product.Id && x.IsConfirmed && !x.IsCancelled)
                .Select(x => new CommentQueryModel
                {
                    Name = x.Name,
                    Message = x.Message,
                    CreationDate = x.CreationDate.ToFarsi(),
                    ChildId = x.ChildId,
                    ParentName = (x.Child != null ? x.Child.Name : string.Empty),
                    ParentMessage = (x.Child != null ? x.Child.Message : string.Empty),
                    ParentCreationDate = (x.Child != null ? x.Child.CreationDate.ToFarsi() : string.Empty)
                })
                .AsNoTracking()
                .ToList();

            var discount = _discountContext
                .CustomerDiscounts
                .Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date)
                .Where(x => x.ProductId == product.Id)
                .OrderByDescending(x => x.Id)
                .AsNoTracking()
                .FirstOrDefault();

            var inventory = _inventoryContext
                .Inventory
                .Where(x => x.ProductId == product.Id)
                .AsNoTracking()
                .SingleOrDefault();

            double price = (inventory != null ? inventory.UnitPrice : 0);

            if (inventory != null)
            {
                product.IsInStock = inventory.InStock;

                product.Price = price.ToMoney();
                product.DoublePrice = price;
            }

            if (discount != null)
            {
                var discountRate = discount.DiscountRate;
                product.DiscountRate = discountRate;
                product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                product.HasDiscount = discountRate > 0;
                var discountAmount = Math.Round(price * discountRate / 100);
                product.PriceWithDiscount = (price - discountAmount).ToMoney();
            }

            return product;
        }

        private static List<ProductPictureQueryModel> MapProductPictures(List<ProductPicture> productPictures)
        {
            return productPictures
                .Where(x => !x.IsRemoved)
                .Select(x => new ProductPictureQueryModel
                {
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PicutreAlt = x.PicutreAlt,
                    ProductId = x.ProductId
                })
                .ToList();
        }

        public (List<ProductQueryModel>, PaginationResult) Search(PaginationOptions paginationOptions, string value)
        {
            var productsQuery = _shopContext
               .Products
               .Where(x =>
               x.Name.Contains(value) ||
               x.Code.Contains(value) ||
               x.KeyWords.Contains(value) ||
               x.ShortDescription.Contains(value))
               .Skip(paginationOptions.PageSize * (paginationOptions.PageNumber - 1))
               .Take(paginationOptions.PageSize)
               .OrderByDescending(x => x.Id)
               .AsNoTracking();

            var selectedData = productsQuery
             .Select(x => new ProductQueryModel
             {
                 Id = x.Id,
                 Name = x.Name,
                 Picture = x.Picture,
                 PictureAlt = x.PictureAlt,
                 PictureTitle = x.PictureTitle,
                 Slug = x.Slug
             });

            var products = selectedData
                .ToList();


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
                    .SingleOrDefault(x => x.ProductId == product.Id);

                double price = 0;

                if (productInventory != null)
                {
                    price = productInventory.UnitPrice;
                    product.IsInStock = productInventory.InStock;
                    product.Price = price.ToMoney();
                }

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

            PaginationResult paginationResult = new();

            paginationResult.PageNumber = paginationOptions.PageNumber;

            paginationResult.ProductCount = GetSearchProductsCount(value);

            paginationResult.TotalPage = (long)Math.Ceiling(double.Parse(paginationResult.ProductCount.ToString()) / paginationOptions.PageSize);

            return (products, paginationResult);
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

        public (List<ProductQueryModel>, PaginationResult) GetDiscountedProducts(PaginationOptions paginationOptions)
        {
            var productDiscount = _discountContext
                .CustomerDiscounts
                .Where(x => x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date)
                .Select(x => new
                {
                    ProudctId = x.ProductId,
                    DiscountRate = x.DiscountRate,
                    EndDate = x.EndDate
                })
                .Skip(paginationOptions.PageSize * (paginationOptions.PageNumber - 1))
                .Take(paginationOptions.PageSize)
                .ToList();

            List<ProductQueryModel> products = new(productDiscount.Count);

            for (int i = 0; i < productDiscount.Count; i++)
            {
                products.Add(GetProductsBy(productDiscount[i].ProudctId));
            }


            var ProductsId = GetProductsId(products);



            var inventories = _inventoryContext
               .Inventory
               .Where(x => ProductsId.Any(z => z == x.ProductId))
               .Select(x => new
               {
                   x.ProductId,
                   x.UnitPrice,
                   x.InStock
               })
               .AsNoTracking();

            foreach (var product in products)
            {

                var discount = productDiscount.FirstOrDefault(x => x.ProudctId == product.Id);

                int discountRate = 0;

                if (discount != null)
                {
                    discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = discountRate > 0;
                }

                var productInventory = inventories
                 .SingleOrDefault(x => x.ProductId == product.Id);

                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.IsInStock = productInventory.InStock;
                    product.Price = price.ToMoney();
                    var discountAmount = Math.Round((price * discountRate) / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }

            PaginationResult paginationResult = new();

            paginationResult.PageNumber = paginationOptions.PageNumber;

            paginationResult.ProductCount = products.Count;

            paginationResult.TotalPage = (long)Math.Ceiling(double.Parse(paginationResult.ProductCount.ToString()) / paginationOptions.PageSize);

            return (products, paginationResult);
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

        private long GetSearchProductsCount(string value)
        {
            return _shopContext
                .Products
                .Where(x =>
                   x.Name.Contains(value) ||
                   x.Code.Contains(value) ||
                   x.KeyWords.Contains(value) ||
                   x.ShortDescription.Contains(value))
                .Count();
        }
        private ProductQueryModel GetProductsBy(long id)
        {
            return _shopContext
                .Products
                .Where(x => x.Id == id)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug
                })
                .SingleOrDefault();
        }

        public List<ProductQueryModel> GetRelatedPrdoucts(string categorySlug)
        {
            var products = _shopContext
                .Products
                .Where(x => x.Category.Slug == categorySlug)
                .OrderBy(x => Guid.NewGuid())
                .Take(7)
                .Include(x => x.Category)
                .Select(x => new ProductQueryModel
                {
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,
                })
                .ToList();


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


            return products;
        }
    }
}
