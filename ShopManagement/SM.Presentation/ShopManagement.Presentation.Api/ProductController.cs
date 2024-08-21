using _01_Query.Contract.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopManagement.Presentation.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductQuery _productQuery;

        public ProductController(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        [HttpGet]
        public List<ProductQueryModel> GetLatestArrivals()
        {
            return null;
            //return _productQuery.GetPopularProducts();
        }

        [HttpPost("GetProductsCheckout")]
        public List<ProductQueryModel> GetProductsby([FromBody] long[] productsId)
        {
            return _productQuery.GetCartsItemByProducts(productsId);
        }
    }
}
