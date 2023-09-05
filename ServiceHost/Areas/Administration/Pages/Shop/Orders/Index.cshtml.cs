using _01_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Configuration.Permissions;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.Orders
{
    public class IndexModel : PageModel
    {
        public OrderSearchModel SearchModel;
        public SelectList Accounts;
        public List<OrderViewModel> Orders;

        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;

        public IndexModel(IOrderApplication orderApplication, IAccountApplication accountApplication)
        {
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
        }

        [NeedPermission(ShopPermissions.ListOrders)]
        public void OnGet(OrderSearchModel searchModel)
        {
            Accounts = new(_accountApplication.GetAccounts(), "Id", "Fullname");
            Orders = _orderApplication.Search(searchModel);
        }

        [NeedPermission(ShopPermissions.ConfirmOrder)]
        public IActionResult OnGetConfirm(long orderId)
        {
            _orderApplication.PaymentSucceeded(orderId, 0);
            return RedirectToPage("./Index");
        }

        [NeedPermission(ShopPermissions.CancelOrder)]
        public IActionResult OnGetCancel(long orderId)
        {
            _orderApplication.Cancel(orderId);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetItems(long orderId)
        {
            var items = _orderApplication.GetItems(orderId);
            return Partial("Items", items);
        }
    }
}