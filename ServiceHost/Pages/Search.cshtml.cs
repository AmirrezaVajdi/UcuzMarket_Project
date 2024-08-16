using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
using _01_Query.Contract.Product;
using _01_Query.Contract.ProductCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class SearchModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        public string Value;
        public (List<ProductQueryModel>, PaginationResult) Products { get; set; }
        public SearchModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(string value, int pageNumber = 1)
        {
            PaginationOptions paginationOptions = new(PageNumber: pageNumber);

            Products = _productQuery.Search(paginationOptions, value);
            Value = value;

            var href = $"""href="{Url.Page("Search", new { value = "1234", pageNumber = 1 })}" """;
        }
    }
}
