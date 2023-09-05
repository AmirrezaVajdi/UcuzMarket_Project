using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using _01_Query.Contract.Product;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems;
        private const string CookieName = "cart-items";
        private readonly IProductQuery _productQuery;

        public CartModel(IProductQuery productQuery)
        {
            _productQuery = productQuery;
            CartItems = new();
        }

        public void OnGet()
        {
            var value = Request.Cookies[CookieName];
            if (value is null) { CartItems = new(); return; }
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value);
            foreach (var cart in cartItems)
                cart.CalculateTotalItemPrice();
            CartItems = _productQuery.CheckInventoryStatus(cartItems);
        }

        public IActionResult OnGetRemove(long id)
        {
            //var value = Request.Cookies[CookieName];
            //Response.Cookies.Delete(CookieName);
            //var items = JsonConvert.DeserializeObject<List<CartItem>>(value);
            //var itemToRemove = items.FirstOrDefault(x => x.Id == id);

            //CookieOptions options = new()
            //{
            //    Expires = DateTime.Now.AddDays(2)
            //};

            //Response.Cookies.Append(CookieName, JsonConvert.SerializeObject(items), options);

            //return RedirectToPage("./Cart");


            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value);
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
            cartItems.Remove(itemToRemove);
            var options = new CookieOptions { Expires = DateTime.Now.AddDays(2) };
            Response.Cookies.Append(CookieName, JsonConvert.SerializeObject(CartItems), options);
            return RedirectToPage("/Cart");
        }

        public IActionResult OnGetGoToCheckOut()
        {
            var value = Request.Cookies[CookieName];
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(value);
            foreach (var cart in cartItems)
            {
                cart.CalculateTotalItemPrice();
            }

            CartItems = _productQuery.CheckInventoryStatus(cartItems);
            if (CartItems.Any(x => !x.IsInStock))
                return RedirectToPage("./Cart");

            return RedirectToPage("./CheckOut");
        }
    }
}
