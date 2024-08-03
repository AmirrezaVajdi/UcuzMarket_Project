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

        public ProductQueryModel Product;
        public List<ProductQueryModel> RelatedProducts { get; set; }

        public ProductModel(IProductQuery productQuery, ICommentApplication commentApplication)
        {
            _productQuery = productQuery;
            _commentApplication = commentApplication;
        }

        public void OnGet(string id)
        {
            Product = _productQuery.GetProductDetails(id);
            RelatedProducts = _productQuery.GetRelatedPrdoucts(Product.CategorySlug);
        }

        public IActionResult OnPost(AddComment comment, string productSlug)
        {
            comment.Type = CommentType.Product;
            var result = _commentApplication.Add(comment);
            return RedirectToPage("./Product", new { id = productSlug });
        }
    }
}
