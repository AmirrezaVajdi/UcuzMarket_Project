using _01_Framework.Application;
using _01_Framework.Infrastructure;
using DiscountManagment.Application.Contract.ColleagueDiscount;
using DiscountManagment.Application.Contract.CustomerDiscount;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Infrastructure.Configuration.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using ServiceHost.Pages.Discounts.CustomerDiscount;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Inventory
{
    [Authorize(Roles = Roles.Administrator)]
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public InventorySearchModel SearchModel { get; set; }
        public List<InventoryViewModel> Inventory
        { get; set; }
        public SelectList Products { get; set; }
        private IProductApplication _productApplication;
        private IInventoryApplication _inventoryApplication;


        public IndexModel(IProductApplication productApplication, IInventoryApplication inventoryApplication)
        {
            _productApplication = productApplication;
            _inventoryApplication = inventoryApplication;
        }

        [NeedPermission(InventoryPermissions.ListInventory)]
        public void OnGet(InventorySearchModel searchModel)
        {
            Products = new(_productApplication.GetProducts(), "Id", "Name");
            Inventory = _inventoryApplication.Search(searchModel);
        }

        [NeedPermission(InventoryPermissions.CreateInventory)]
        public IActionResult OnGetCreate()
        {
            CreateInventory command = new()
            {
                Products = _productApplication.GetProducts()
            };
            return Partial("./Create", command);
        }

        [NeedPermission(InventoryPermissions.CreateInventory)]
        public JsonResult OnPostCreate(CreateInventory command)
        {
            var result = _inventoryApplication.Create(command);
            return new JsonResult(result);
        }

        [NeedPermission(InventoryPermissions.EditInventory)]
        public IActionResult OnGetEdit(long id)
        {
            var inventory = _inventoryApplication.GetDetails(id);
            inventory.Products = _productApplication.GetProducts();
            return Partial("./Edit", inventory);
        }

        [NeedPermission(InventoryPermissions.EditInventory)]
        public JsonResult OnPostEdit(EditInventory command)
        {
            var result = _inventoryApplication.Edit(command);
            return new JsonResult(result);
        }

        [NeedPermission(InventoryPermissions.Increase)]
        public IActionResult OnGetIncrease(long Id)
        {
            IncreaseInventory increase = new()
            {
                InventoryId = Id
            };
            return Partial("Increase", increase);
        }

        [NeedPermission(InventoryPermissions.Increase)]
        public IActionResult OnPostIncrease(IncreaseInventory command)
        {
            var result = _inventoryApplication.Increase(command);
            return new JsonResult(result);
        }

        [NeedPermission(InventoryPermissions.Reduce)]
        public IActionResult OnGetReduce(long Id)
        {
            ReduceInventory reduce = new()
            {
                InventoryId = Id
            };
            return Partial("Reduce", reduce);
        }

        [NeedPermission(InventoryPermissions.Reduce)]
        public IActionResult OnPostReduce(ReduceInventory command)
        {
            var result = _inventoryApplication.Reduce(command);
            return new JsonResult(result);
        }

        [NeedPermission(InventoryPermissions.OperationLog)]
        public IActionResult OngetLog(long Id)
        {
            var log = _inventoryApplication.GetOperationLog(Id);
            return Partial("./OperationLog", log);
        }
    }
}