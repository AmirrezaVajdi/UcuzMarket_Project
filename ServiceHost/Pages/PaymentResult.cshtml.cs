using _0_Framework.Application.ZarinPal;
using _01_Query.Contract.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    [Authorize]
    public class PaymentResultModel : PageModel
    {
        private readonly IProductQuery _productQuery;

        public PaymentResult Result;
        public List<ProductQueryModel> Products { get; set; }

        public PaymentResultModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
        }

        public IActionResult OnGet(PaymentResult result)
        {
            if (result.IssueTrackingNo == null)
                return RedirectToPage("./Index");
            else
            {
                Result = result;
                Products = _productQuery.GetRelatedPrdoucts(take: 5);
                return Page();
            }
        }
    }
}
