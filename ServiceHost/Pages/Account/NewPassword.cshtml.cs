using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Account
{
    public class NewPasswordModel : PageModel
    {
        private PasswordRecoveyTokenService _passwordRecoveyTokenService;
        private IAccountApplication _accountApplication;

        public PasswordRecoveryDto PasswordRecoveryDto { get; set; }

        [BindProperty]
        public ChangePassword ChangePasswordModel { get; set; }

        [BindProperty]
        public Guid Guid { get; set; }

        public NewPasswordModel(PasswordRecoveyTokenService passwordRecoveyTokenService, IAccountApplication accountApplication)
        {
            _passwordRecoveyTokenService = passwordRecoveyTokenService;
            _accountApplication = accountApplication;
        }

        public IActionResult OnGet([FromQuery] Guid Id)
        {
            PasswordRecoveryDto = _passwordRecoveyTokenService.GetBy(Id);

            if (Id != Guid.Empty && PasswordRecoveryDto != null)
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
                PasswordRecoveryDto = _passwordRecoveyTokenService.GetBy(Guid);

                ChangePasswordModel.Id = _accountApplication.GetAccountBy(PasswordRecoveryDto.PhoneNumber).Id;

                var result = _accountApplication.ChangePassword(ChangePasswordModel);

                if (result.isSuccedded)
                {
                    _passwordRecoveyTokenService.Done(PasswordRecoveryDto.Guid);
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
