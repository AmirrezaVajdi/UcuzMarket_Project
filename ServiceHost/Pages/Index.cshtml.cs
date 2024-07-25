using DeliveryManagement.Application.Contract.Delivery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDeliveryApplication _deliveryApplication;

        public IndexModel(IDeliveryApplication deliveryApplication)
        {
            _deliveryApplication = deliveryApplication;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostCreateDelivery(CreateDelivery createDelivery)
        {
            return new OkResult();
        }

        public JsonResult OnPostSetDefaultDelivery(SetToDefaultDelivery setToDefaultDelivery)
        {
            var result = _deliveryApplication.SetToDefaultDelivery(setToDefaultDelivery);
            return new JsonResult(result);
        }
    }
}