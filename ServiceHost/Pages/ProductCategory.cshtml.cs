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

        [BindProperty]
        public PaginationOptions PaginationOptions { get; set; }

        [BindProperty]
        public string CurentPageSlug { get; set; }

        [BindProperty]
        public string SelectedFilter { get; set; }

        public ProductCategoryModel(IProductCategoryQuery productCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
        }

        public void OnGet(string id, int pageNumber = 1)
        {
            PaginationOptions = new(PageNumber: pageNumber);

            ProductCategory = _productCategoryQuery.GetProductCategoryWithProductsBy(id, PaginationOptions);
        }

        public IActionResult OnPost()
        {
            //if (SelectedFilter != null)
            //{
            //    switch (SelectedFilter)
            //    {
            //        case "1":
            //            {
            //                SortingOptions = new(x => x.unitpr);
            //                break;
            //            }
            //    }

            //}

            //ProductCategory = _productCategoryQuery.GetProductCategoryWithProductsBy(CurentPageSlug, PaginationOptions, FilteringOptions, SortingOptions, ProductStatusOptions);
            return RedirectToPage("/");
        }
    }
}
