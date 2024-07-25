using _01_Framework.Application;
using DeliveryManagement.Application.Contract.Delivery;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DeliveryManagement.Presentation.API
{
    [Route("api/{controller}")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryApplication _deliveryApplication;
        private IAuthHelper _helper;

        public DeliveryController(IDeliveryApplication deliveryApplication, IAuthHelper helper)
        {
            _deliveryApplication = deliveryApplication;
            _helper = helper;
        }

        [HttpPost("SetDefaultAddress")]
        public string SetDefaultAddress([FromBody] SetToDefaultDelivery command)
        {
            if (_helper.IsAuthenticated())
            {
                var result = _deliveryApplication.SetToDefaultDelivery(command);
                return JsonConvert.SerializeObject(result);
            }
            return "Please Login to System";
        }
    }
}
