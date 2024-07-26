using _01_Framework.Application;
using DeliveryManagement.Application.Contract.Delivery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IAuthHelper _helper;

        public IndexModel(IDeliveryApplication deliveryApplication, IAuthHelper helper)
        {
            _deliveryApplication = deliveryApplication;
            _helper = helper;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostCreateDelivery(CreateDelivery createDelivery)
        {
            if (_helper.IsAuthenticated())
            {
                createDelivery.AccountId = _helper.CurrentAccountId();
                _deliveryApplication.Create(createDelivery);
            }
            return RedirectToPage("./Index");
        }
    }
}