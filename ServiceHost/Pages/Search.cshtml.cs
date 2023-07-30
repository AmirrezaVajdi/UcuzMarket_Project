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
        public List<ProductQueryModel> Products { get; set; }
        public SearchModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public void OnGet(string value)
        {
            Products = _productQuery.Search(value);
            Value = value;
        }
    }
}
