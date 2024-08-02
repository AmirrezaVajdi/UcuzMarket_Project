using _01_Framework.Application.Pagination;
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
            PaginationOptions paginationOptions = new(PageSize: 8);
            var products = _productQuery.GetPopularProducts(paginationOptions);
            return View(products);
        }
    }
}
