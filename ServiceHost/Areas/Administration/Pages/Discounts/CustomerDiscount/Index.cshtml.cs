using _01_Framework.Application;
using DiscountManagement.Configuration.Permissions;
using DiscountManagment.Application.Contract.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceHost.Pages.Discounts.CustomerDiscount;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Discounts.CustomerDiscount
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public CustomerDiscountSearchModel SearchModel { get; set; }
        public List<CustomerDiscountViewModel> CustomerDiscounts
        { get; set; }
        public SelectList Products { get; set; }
        private IProductApplication _productApplication;
        private ICustomerDiscountApplication _customerDiscountApplication;


        public IndexModel(IProductApplication productApplication, ICustomerDiscountApplication customerDiscountApplication)
        {
            _productApplication = productApplication;
            _customerDiscountApplication = customerDiscountApplication;
        }

        [NeedPermission(DiscountPermission.ListCustomerDiscounts)]
        public void OnGet(CustomerDiscountSearchModel searchModel)
        {
            Products = new(_productApplication.GetProducts(), "Id", "Name");
            CustomerDiscounts = _customerDiscountApplication.Search(searchModel);
        }

        [NeedPermission(DiscountPermission.DefineCustomerDiscount)]
        public IActionResult OnGetCreate()
        {
            DefineCustomerDiscount command = new()
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        [NeedPermission(DiscountPermission.DefineCustomerDiscount)]
        public JsonResult OnPostCreate(DefineCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Define(command);
            return new JsonResult(result);
        }

        [NeedPermission(DiscountPermission.EditCustomerDiscount)]
        public IActionResult OnGetEdit(long id)
        {
            var customerDiscount = _customerDiscountApplication.GetDetails(id);
            customerDiscount.Products = _productApplication.GetProducts();
            return Partial("./Edit", customerDiscount);
        }

        [NeedPermission(DiscountPermission.EditCustomerDiscount)]
        public JsonResult OnPostEdit(EditCustomerDiscount command)
        {
            var result = _customerDiscountApplication.Edit(command);
            return new JsonResult(result);
        }
    }
}
