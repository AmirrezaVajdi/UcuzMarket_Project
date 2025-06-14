using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class EditModel : PageModel
    {
        public List<SelectListItem> Permissions = new();
        public EditRole Command;
        private IRoleApplication _roleApplication;
        private readonly IEnumerable<IPermissionExposer> _exposers;


        public EditModel(IRoleApplication roleApplication, IEnumerable<IPermissionExposer> exposers)
        {
            _roleApplication = roleApplication;
            _exposers = exposers;
        }

        [NeedPermission(AccountPermission.EditRole)]
        public void OnGet(long id)
        {
            Command = _roleApplication.GetDetails(id);
            var permissions = new List<PermissionDto>();

            foreach (var exposer in _exposers)
            {
                var exposedPermissions = exposer.Expose();

                foreach (var (key, value) in exposedPermissions)
                {
                    permissions.AddRange(value);

                    var group = new SelectListGroup()
                    {
                        Name = key
                    };

                    foreach (var permission in value)
                    {
                        var item = new SelectListItem(permission.Name, permission.Code.ToString())
                        {
                            Group = group
                        };

                        if (Command.MappedPermissions.Any(x => x.Code == permission.Code))
                            item.Selected = true;

                        Permissions.Add(item);
                    }
                }
            }
        }

        [NeedPermission(AccountPermission.EditRole)]
        public IActionResult OnPost(EditRole command)
        {
            var result = _roleApplication.Edit(command);
            return RedirectToPage("./Index");
        }
    }
}
