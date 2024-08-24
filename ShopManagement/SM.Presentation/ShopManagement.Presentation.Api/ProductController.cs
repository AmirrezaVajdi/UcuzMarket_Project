using _01_Query;
using _01_Query.Contract.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.Order;

namespace ShopManagement.Presentation.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductQuery _productQuery;
        private readonly ICartService _cartService;
        private readonly ICartCalculatorService _cartCalculatorService;

        public ProductController(IProductQuery productQuery, ICartService cartService, ICartCalculatorService cartCalculatorService)
        {
            _productQuery = productQuery;
            _cartService = cartService;
            _cartCalculatorService = cartCalculatorService;
        }

        [HttpGet]
        public List<ProductQueryModel> GetLatestArrivals()
        {
            return null;
            //return _productQuery.GetPopularProducts();
        }

        [HttpPost("GetProductsCheckout")]
        public List<ProductQueryModel> GetProductsby([FromBody] List<CheckoutModel> models)
        {
            var productsId = new List<long>(models.Count);

            models.ForEach(x => productsId.Add(x.productId));

            var products = _productQuery.GetCartsItemByProducts(productsId.ToArray());


            var cartItems = new List<CartItem>(productsId.Count);

            for (int i = 0; i < productsId.Count; i++)
            {
                var product = products.First(x => x.Id == models[i].productId);
                var cartitem = new CartItem()
                {
                    Id = product.Id,
                    Count = models[i].count,
                    DiscountRate = product.DiscountRate,
                    UnitPrice = double.Parse(product.Price)
                };

                cartitem.CalculateTotalItemPrice();

                cartItems.Add(cartitem);

            }

            Cart cart = _cartCalculatorService.ComputeCart(cartItems);

            _cartService.Set(cart);

            return products;
        }

        [HttpPost("CheckInventoryStatus")]
        public bool CheckInventoryStatusBy([FromBody] CheckInventoryModel model)
        {
            return _productQuery.CheckInventoryStatusBy(model.ProductId, model.Count);
        }
    }
    public class CheckoutModel
    {
        public long productId { get; set; }
        public int count { get; set; }
    }

    public class CheckInventoryModel
    {
        public long ProductId { get; set; }
        public int Count { get; set; }
    }
}
