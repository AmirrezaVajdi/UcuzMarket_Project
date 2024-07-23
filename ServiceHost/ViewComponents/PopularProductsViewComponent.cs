using _01_Query.Contract.Product;
using _01_Query.Contract.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class PopularProductsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public PopularProductsViewComponent(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var products = _productQuery.GetPopularProducts();
            return View(products);
        }
    }
}
