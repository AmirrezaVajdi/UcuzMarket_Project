using _01_Framework.Application;
using DiscountManagement.Configuration.Permissions;
using DiscountManagment.Application.Contract.ColleagueDiscount;
using DiscountManagment.Application.Contract.CustomerDiscount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceHost.Pages.Discounts.CustomerDiscount;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Discounts.ColleagueDiscount
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public ColleagueDiscountSearchModel SearchModel { get; set; }
        public List<ColleagueDiscountViewModel> Inventory
        { get; set; }
        public SelectList Products { get; set; }
        private IProductApplication _productApplication;
        private IColleagueDiscountApplication _ColleagueDiscountApplication;


        public IndexModel(IProductApplication productApplication, IColleagueDiscountApplication colleagueDiscountApplication)
        {
            _productApplication = productApplication;
            _ColleagueDiscountApplication = colleagueDiscountApplication;
        }

        [NeedPermission(DiscountPermission.ListColleagueDiscounts)]
        public void OnGet(ColleagueDiscountSearchModel searchModel)
        {
            Products = new(_productApplication.GetProducts(), "Id", "Name");
            Inventory = _ColleagueDiscountApplication.Search(searchModel);
        }

        [NeedPermission(DiscountPermission.DefineColleagueDiscount)]
        public IActionResult OnGetCreate()
        {
            DefineColleagueDiscount command = new()
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        [NeedPermission(DiscountPermission.DefineColleagueDiscount)]
        public JsonResult OnPostCreate(DefineColleagueDiscount command)
        {
            var result = _ColleagueDiscountApplication.Define(command);
            return new JsonResult(result);
        }

        [NeedPermission(DiscountPermission.EditColleagueDiscount)]
        public IActionResult OnGetEdit(long id)
        {
            var colleagueDiscoutn = _ColleagueDiscountApplication.GetDetails(id);
            colleagueDiscoutn.Products = _productApplication.GetProducts();
            return Partial("./Edit", colleagueDiscoutn);
        }

        [NeedPermission(DiscountPermission.EditColleagueDiscount)]
        public JsonResult OnPostEdit(EditColleagueDiscount command)
        {
            var result = _ColleagueDiscountApplication.Edit(command);
            return new JsonResult(result);
        }

        [NeedPermission(DiscountPermission.RemoveColleagueDiscount)]
        public IActionResult OnGetRemove(long id)
        {
            _ColleagueDiscountApplication.Remove(id);
            return RedirectToPage("./Index");
        }

        [NeedPermission(DiscountPermission.RemoveColleagueDiscount)]
        public IActionResult OnGetRestore(long id)
        {
            _ColleagueDiscountApplication.Restore(id);
            return RedirectToPage("./Index");
        }

    }
}