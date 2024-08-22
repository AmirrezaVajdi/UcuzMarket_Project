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

        public ProductController(IProductQuery productQuery, ICartService cartService)
        {
            _productQuery = productQuery;
            _cartService = cartService;
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

            return products;
        }
    }
    public class CheckoutModel
    {
        public long productId { get; set; }
        public int count { get; set; }
    }
}
