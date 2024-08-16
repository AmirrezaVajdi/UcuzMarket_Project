using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
using _01_Query.Contract.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class DiscountedProductsModel : PageModel
    {
        private readonly IProductQuery _productQuery;

        public (List<ProductQueryModel>, PaginationResult) Products { get; set; }
        public DiscountedProductsModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(int pageNumber = 1)
        {
            PaginationOptions paginationOptions = new(PageSize: 16, PageNumber: pageNumber);
            Products = _productQuery.GetDiscountedProducts(paginationOptions);
        }
    }
}
