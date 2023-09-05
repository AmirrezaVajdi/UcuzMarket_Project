using _01_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class AccountModel : PageModel
    {
        [TempData]
        public string RegisterMessage { get; set; }

        [TempData]
        public string LoginMessage { get; set; }
        private IAccountApplication _accountApplication;

        public AccountModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostLogin(Login command)
        {
            if (!LoginModelIsNull(command))
            {
                var result = _accountApplication.Login(command);
                if (result.isSuccedded)
                    return RedirectToPage("./Index");

                LoginMessage = "نام کاربری یا رمز عبور اشتباه می باشد";
                TempData["message"] = "نام کاربری یا رمز عبور اشتباه می باشد";

                return RedirectToPage("./Account");
            }

            LoginMessage = "نام کاربری و رمز عبور اجباری می باشد";
            return RedirectToPage("./Account");
        }

        public IActionResult OnGetLogout()
        {
            _accountApplication.Logout();
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostRegister(RegisterAccount command)
        {
            if (!RegisterAccountIsNull(command))
            {
                var result = _accountApplication.Register(command);
                if (result.isSuccedded)
                {
                    RegisterMessage = "ثبت نام شما با موفقیت انجام شد . میتوانید وارد فروشگاه شوید";
                    return RedirectToPage("./Account");
                }
                RegisterMessage = result.Message;
                return RedirectToPage("./Account");
            }
            RegisterMessage = "همه مقادیر باید پر شده باشند";
            return RedirectToPage("./Account");
        }

        public bool LoginModelIsNull(Login login)
        {
            return login.Username is null || login.Password is null;
        }

        public bool RegisterAccountIsNull(RegisterAccount register)
        {
            return register.Fullname is null || register.Username is null || register.Password is null || register.Mobile is null;
        }
    }
}
