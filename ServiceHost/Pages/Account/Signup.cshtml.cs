using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Account
{
    public class SignupModel : PageModel
    {
        private IAccountApplication _accountApplication;

        [TempData]
        public string LoginMessage { get; set; }

        public RegisterAccount RegisterAccountModel { get; set; }

        public SignupModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(RegisterAccount command)
        {
            if (ModelState.IsValid)
            {
                var isRegister = _accountApplication.Register(command);
                if (isRegister.isSuccedded)
                {
                    return RedirectToPage("./Signin");
                }
                else
                {
                    LoginMessage = isRegister.Message;
                    return Page();
                }
            }
            LoginMessage = "لطفا فیلد های اجباری را وارد کنید";
            return Page();
        }
    }
}
