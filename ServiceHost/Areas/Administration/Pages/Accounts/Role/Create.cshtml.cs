using _01_Framework.Application;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class CreateModel : PageModel
    {
        public CreateRole Command;
        private readonly IRoleApplication _roleApplication;

        public CreateModel(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        [NeedPermission(AccountPermission.CreateRole)]
        public void OnGet()
        {
        }

        [NeedPermission(AccountPermission.CreateRole)]
        public IActionResult OnPost(CreateRole command)
        {
            _roleApplication.Create(command);
            return RedirectToPage("./Index");
        }
    }
}
