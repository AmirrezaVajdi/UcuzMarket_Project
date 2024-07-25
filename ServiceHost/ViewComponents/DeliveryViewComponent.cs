using _01_Framework.Application;
using DeliveryManagement.Application.Contract.Delivery;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class DeliveryViewComponent : ViewComponent
    {
        private readonly IDeliveryApplication _deliveryApplication;
        private readonly IAuthHelper _authHelper;
        public DeliveryViewComponent(IDeliveryApplication deliveryApplication, IAuthHelper authHelper)
        {
            _deliveryApplication = deliveryApplication;
            _authHelper = authHelper;
        }

        public IViewComponentResult Invoke()
        {
            var deliveries = _deliveryApplication.List(_authHelper.CurrentAccountId());
            return View(deliveries);
        }
    }
}
