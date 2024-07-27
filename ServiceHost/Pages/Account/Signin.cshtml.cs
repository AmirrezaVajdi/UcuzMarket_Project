using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Account
{
    public class SigninModel : PageModel
    {
        private IAccountApplication _accountApplication;


        [BindProperty]
        public Login LoginModel { get; set; }

        public SigninModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var isLogin = _accountApplication.Login(LoginModel);
                if (isLogin.isSuccedded)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    TempData["LoginMessage"] = isLogin.Message;
                    return Page();
                }
            }
            TempData["LoginMessage"] = "لطفا فیلد های اجباری را وارد کنید";
            return Page();
        }
    }
}
