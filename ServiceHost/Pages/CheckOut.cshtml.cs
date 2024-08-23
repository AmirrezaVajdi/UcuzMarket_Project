using _0_Framework.Application.ZarinPal;
using _01_Framework.Application;
using _01_Query;
using _01_Query.Contract.Product;
using _01_Query.Query;
using DeliveryManagement.Application.Contract.Delivery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckOutModel : PageModel
    {
        //public Cart Cart;

        //private const string CookieName = "cart-items";

        //  
        //private readonly IProductQuery _productQuery;
        //private readonly ICartService _cartService;
        //private readonly IOrderApplication _orderApplication;
        //private readonly IZarinPalFactory _zarinPalFactory;
        //private readonly IAuthHelper _authHelper;

        //public CheckOutModel(ICartCalculatorService cartCalculatorService, ICartService cartService, IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory, IAuthHelper authHelper)
        //{
        //    _cartCalculatorService = cartCalculatorService;
        //    _cartService = cartService;
        //    _productQuery = productQuery;
        //    _orderApplication = orderApplication;
        //    _zarinPalFactory = zarinPalFactory;
        //    _authHelper = authHelper;
        //    Cart = new();
        //}

        //public void OnGet()
        //{
        //    var value = Request.Cookies[CookieName];
        //    var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value);
        //    foreach (var cart in cartItems)
        //        cart.CalculateTotalItemPrice();

        //    Cart = _cartCalculatorService.ComputeCart(cartItems);
        //    _cartService.Set(Cart);
        //}

        //public IActionResult OnPostPay(int paymentMethod)
        //{
        //    var cart = _cartService.Get();
        //    cart.SetPaymentMethod(paymentMethod);
        //    var result = _productQuery.CheckInventoryStatus(cart.Items);
        //    if (result.Any(x => !x.IsInStock))
        //        return RedirectToPage("./Cart");

        //    var orderId = _orderApplication.PlaceOrder(cart);

        //    if (paymentMethod == 1)
        //    {
        //        var paymentResponse = _zarinPalFactory.CreatePaymentRequest(cart.PayAmmount.ToString(), "", "", "خرید از لوازم خانگی و دکوری", orderId);

        //        return Redirect($"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
        //    }

        //    PaymentResult paymentResult = new();
        //    return RedirectToPage("./PaymentResult", paymentResult.Succeeded("سفارش شما با موفقیت ثبت شد . پس از تماس کارشناسان ما و پرداخت وجه ، سفارش شما ارسال خواهد شد", null));

        //}
        //public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status, [FromQuery] long oId)
        //{
        //    var orderAmout = _orderApplication.GetAmountBy(oId);
        //    var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority, orderAmout.ToString());

        //    PaymentResult result = new();

        //    if (status == "OK" && verificationResponse.Status >= 100)
        //    {
        //        var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
        //        Response.Cookies.Delete(CookieName);
        //        result = result.Succeeded("پرداخت با موفقیت انجام شد", issueTrackingNo);
        //        return RedirectToPage("./PaymentResult", result);
        //    }

        //    result = result.Failed("پرداخت با موفقیت انجام نشد . در صورت کسر مبلغ تا حداکثر 72 ساعت به حساب شما بازگردانده خواهد شد");
        //    return RedirectToPage("./PaymentResult", result);
        //}






        private readonly IProductQuery _productQuery;
        private readonly ICartService _cartService;
        private readonly IOrderApplication _orderApplication;
        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IAuthHelper _authHelper;
        private readonly IDeliveryApplication _deliveryApplication;

        public DeliveryViewModel Delivery { get; set; }

        public CheckOutModel(ICartService cartService, IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory, IAuthHelper authHelper, IDeliveryApplication deliveryApplication)
        {
            _cartService = cartService;
            _productQuery = productQuery;
            _orderApplication = orderApplication;
            _zarinPalFactory = zarinPalFactory;
            _authHelper = authHelper;
            _deliveryApplication = deliveryApplication;
        }

        public void OnGet()
        {

        }

        public IActionResult OnGetPay(int paymentMethod = 1)
        {
            var cart = _cartService.Get();
            cart.SetPaymentMethod(paymentMethod);

            var result = _productQuery.CheckInventoryStatus(cart.Items);
            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("./Cart");


            var delivery = _deliveryApplication.GetDefaultDeliveryBy(_authHelper.CurrentAccountId());
            var address = "میاندوآب ، " + delivery.Address + " ، " + delivery.PostalCode;

            var orderId = _orderApplication.PlaceOrder(cart, address);

            if (paymentMethod == 1)
            {
                var paymentResponse = _zarinPalFactory.CreatePaymentRequest(cart.PayAmmount.ToString(), "", "", "اوجوز مارکت", orderId);

                return Redirect($"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponse.Authority}");
            }

            PaymentResult paymentResult = new();
            return RedirectToPage("./PaymentResult", paymentResult.Succeeded("سفارش شما با موفقیت ثبت شد . پس از تماس کارشناسان ما و پرداخت وجه ، سفارش شما ارسال خواهد شد", null));

        }
        public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status, [FromQuery] long oId)
        {
            var orderAmout = _orderApplication.GetAmountBy(oId);
            var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority, orderAmout.ToString());

            PaymentResult result = new();

            if (status == "OK" && verificationResponse.Status >= 100)
            {
                var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.RefID);
                //باید محصولات از سبد خرید حذف شوند
                result = result.Succeeded("پرداخت با موفقیت انجام شد", issueTrackingNo);
                return RedirectToPage("./PaymentResult", result);
            }

            result = result.Failed("پرداخت با موفقیت انجام نشد . در صورت کسر مبلغ تا حداکثر 72 ساعت به حساب شما بازگردانده خواهد شد");
            return RedirectToPage("./PaymentResult", result);
        }
    }
}
