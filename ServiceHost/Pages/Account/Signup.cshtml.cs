using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Account
{
    public class SignupModel : PageModel
    {
        private IAccountApplication _accountApplication;

        [BindProperty]
        public RegisterAccount RegisterModel { get; set; }

        public SignupModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            ModelState.ClearValidationState("RegisterModel.Roles");
            ModelState.MarkFieldValid("RegisterModel.Roles");

            ModelState.ClearValidationState("RegisterModel.RoleId");
            ModelState.MarkFieldValid("RegisterModel.RoleId");

            if (ModelState.IsValid)
            {
                //User RoleId
                RegisterModel.RoleId = 2;
                var isRegister = _accountApplication.Register(RegisterModel);
                if (isRegister.isSuccedded)
                {
                    return RedirectToPage("./Signin");
                }
                else
                {
                    TempData["RegisterMessage"] = isRegister.Message;
                    return Page();
                }
            }
            TempData["RegisterMessage"] = "لطفا فیلد های اجباری را وارد کنید";
            return Page();
        }
    }
}
