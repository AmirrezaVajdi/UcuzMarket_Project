using _01_Framework.Application.Pagination;
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
            PaginationOptions paginationOptions = new(PageSize: 4);
            var discountedProducts = _productQuery.GetDiscountedProducts(paginationOptions);
            return View(discountedProducts);
        }
    }
}
