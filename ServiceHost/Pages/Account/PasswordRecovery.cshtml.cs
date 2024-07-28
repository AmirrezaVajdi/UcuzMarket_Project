using _01_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ServiceHost.Pages.Account
{
    public class PasswordRecoveryModel : PageModel
    {
        private PasswordRecoveyTokenService _passwordRecoveyTokenService;

        private readonly IAccountApplication _accountApplication;

        public PasswordRecoveryModel(PasswordRecoveyTokenService passwordRecoveyTokenService, IAccountApplication accountApplication)
        {
            _passwordRecoveyTokenService = passwordRecoveyTokenService;
            _accountApplication = accountApplication;
        }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(11, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(11, ErrorMessage = ValidationMessages.MinLenght)]
        [BindProperty]
        public string PhoneNumber { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (_accountApplication.GetAccountBy(PhoneNumber).Mobile != null)
                {
                    _passwordRecoveyTokenService.Guid = Guid.NewGuid();
                    _passwordRecoveyTokenService.PhoneNumber = PhoneNumber;

                    return RedirectToPage("./PhoneNumberVerification", routeValues: new { Id = _passwordRecoveyTokenService.Guid.ToString() });
                }
                else
                {
                    TempData["Message"] = "چنین حساب کاربری وجود ندارد";
                }
            }
            return Page();

        }
    }
}
