using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
using _01_Query.Contract.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class PopularProductsModel : PageModel
    {
        private IProductQuery _productQuery;
        public (List<ProductQueryModel>, PaginationResult) Products { get; set; }

        public PopularProductsModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(int pageNumber = 1)
        {
            PaginationOptions paginationOptions = new(PageNumber: pageNumber);
            Products = _productQuery.GetPopularProducts(paginationOptions);
        }
    }
}
