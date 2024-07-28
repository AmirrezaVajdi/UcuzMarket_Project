using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Account
{
    public class NewPasswordModel : PageModel
    {
        private PasswordRecoveyTokenService _passwordRecoveyTokenService;
        private IAccountApplication _accountApplication;

        [BindProperty]
        public ChangePassword ChangePasswordModel { get; set; }

        public NewPasswordModel(PasswordRecoveyTokenService passwordRecoveyTokenService, IAccountApplication accountApplication)
        {
            _passwordRecoveyTokenService = passwordRecoveyTokenService;
            _accountApplication = accountApplication;
        }

        public IActionResult OnGet([FromQuery] Guid Id)
        {
            if (Id != Guid.Empty && _passwordRecoveyTokenService.Guid == Id)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("./PasswordRecovery");
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                ChangePasswordModel.Id = _accountApplication.GetAccountBy(_passwordRecoveyTokenService.PhoneNumber).Id;

                var result = _accountApplication.ChangePassword(ChangePasswordModel);

                if (result.isSuccedded)
                {
                    return RedirectToPage("/Account/Signin");
                }
                else
                {
                    TempData["Message"] = result.Message;
                    return Page();
                }
            }
            return Page();
        }
    }
}
