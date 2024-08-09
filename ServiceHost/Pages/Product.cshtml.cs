using _01_Framework.Application;
using _01_Query.Contract.Product;
using CommandManagement.Application.Contract.Comment;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Comment;

namespace ServiceHost.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        private readonly ICommentApplication _commentApplication;
        private readonly IAuthHelper _helper;

        public ProductQueryModel Product;
        public List<ProductQueryModel> RelatedProducts { get; set; }

        [BindProperty]
        public AddComment AddComment { get; set; }

        [BindProperty]
        public string ProductSlug { get; set; }

        public ProductModel(IProductQuery productQuery, ICommentApplication commentApplication, IAuthHelper helper)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
            _helper = helper;
        }

        public void OnGet(string id)
        {
            LoadData(id);
        }

        public IActionResult OnPost()
        {
            if (_helper.IsAuthenticated())
            {
                AddComment.Type = CommentType.Product;
                AddComment.AccountId = _helper.CurrentAccountId();
                var result = _commentApplication.Add(AddComment);
                TempData["Message"] = "<span class='text-success text-center d-block'>نظر شما با موفقیت ثبت شد و پس از تایید نمایش داده خواهد شد</span>";

                LoadData(ProductSlug);

                return Page();
                //return RedirectToPage("./Product", new { id = ProductSlug });
            }
            else
            {
                LoadData(ProductSlug);

                TempData["Message"] = "<span class='text-danger text-center d-block'>لطفا برای ثبت نظر وارد سایت شوید</span>";
                return Page();
            }
        }

        private void LoadData(string id)
        {
            Product = _productQuery.GetProductDetails(id);
            RelatedProducts = _productQuery.GetRelatedPrdoucts(Product.CategorySlug);
        }
    }
}
