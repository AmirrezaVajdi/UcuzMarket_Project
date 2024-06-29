using _01_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.ProdcutCategory;
using ShopManagement.Application.Contracts.Product;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public AccountSearchModel SearchModel { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public SelectList Roles { get; set; }

        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {

            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        [NeedPermission(AccountPermission.ListAccount)]
        public void OnGet(AccountSearchModel searchModel)
        {
            Roles = new(_roleApplication.List(), "Id", "Name");
            Accounts = _accountApplication.Search(searchModel);
        }

        [NeedPermission(AccountPermission.RegisterAccount)]
        public IActionResult OnGetCreate()
        {
            var command = new RegisterAccount
            {
                Roles = _roleApplication.List()
            };
            return Partial("./Create", command);
        }

        [NeedPermission(AccountPermission.RegisterAccount)]
        public IActionResult OnPostCreate(RegisterAccount command)
        {
            ModelState.ClearValidationState("Roles");
            ModelState.MarkFieldValid("Roles");

            if (ModelState.IsValid)
            {
                var result = _accountApplication.Register(command);
                return new JsonResult(result);
            }
            command.Roles = _roleApplication.List();
            return Partial("./Create" , command);
        }

        [NeedPermission(AccountPermission.EditAccount)]
        public IActionResult OnGetEdit(long id)
        {
            var account = _accountApplication.GetDetails(id);
            account.Roles = _roleApplication.List();
            return Partial("./Edit", account);
        }

        [NeedPermission(AccountPermission.EditAccount)]
        public JsonResult OnPostEdit(EditAccount command)
        {
            var result = _accountApplication.Edit(command);
            return new JsonResult(result);
        }


        [NeedPermission(AccountPermission.ChangePassword)]
        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword() { Id = id };
            return Partial("./ChangePassword", command);
        }

        [NeedPermission(AccountPermission.ChangePassword)]
        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePassword(command);
            return new JsonResult(result);
        }
    }
}
