using _01_Query.Contract.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class DiscountedProductsViewComponent : ViewComponent
    {
        private readonly IProductQuery _productQuery;

        public DiscountedProductsViewComponent(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public IViewComponentResult Invoke()
        {
            var discountedProducts = _productQuery.GetDiscountedProducts();
            return View(discountedProducts);
        }
    }
}
