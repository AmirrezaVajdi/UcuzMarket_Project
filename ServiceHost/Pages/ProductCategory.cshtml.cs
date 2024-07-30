using _01_Framework.Application.Pagination;
using _01_Framework.Application.Paginations;
using _01_Query.Contract.ProductCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ProductCategoryModel : PageModel
    {
        private readonly IProductCategoryQuery _productCategoryQuery;

        public ProdcutCategoryQueryModel ProductCategory { get; set; }

        public List<ProductCategoryWithChildren> ProductCategoryWithChildren { get; set; }

        [BindProperty]
        public PaginationOptions PaginationOptions { get; set; }

        [BindProperty]
        public FilteringOptions<ProdcutCategoryQueryModel> FilteringOptions { get; set; }

        [BindProperty]
        public SortingOptions<ProdcutCategoryQueryModel> SortingOptions { get; set; }

        [BindProperty]
        public PricingOptions PricingOptions { get; set; }

        public ProductCategoryModel(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public void OnGet(string id)
        {
            PaginationOptions = new();
            ProductCategoryWithChildren = _productCategoryQuery.GetCategoryAndPictureWithChildren();
            ProductCategory = _productCategoryQuery.GetProductCategoryWithProductsBy(id, PaginationOptions, FilteringOptions, SortingOptions);
        }
    }
}
