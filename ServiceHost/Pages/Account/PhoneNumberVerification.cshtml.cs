using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ServiceHost.Pages.Account
{
    public class PhoneNumberVerificationModel : PageModel
    {
        private PasswordRecoveyTokenService _passwordRecoveyTokenService;

        public PhoneNumberVerificationModel(PasswordRecoveyTokenService passwordRecoveyTokenService)
        {
            _passwordRecoveyTokenService = passwordRecoveyTokenService;
        }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(6, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(6, ErrorMessage = ValidationMessages.MinLenght)]
        [BindProperty]
        public string Code { get; set; }

        public IActionResult OnGet([FromQuery] Guid Id)
        {
            if (Id != Guid.Empty && _passwordRecoveyTokenService.Guid == Id)
            {
                //code send function
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
                if (Code == "123456")
                {
                    string phoneNumber = _passwordRecoveyTokenService.PhoneNumber;

                    _passwordRecoveyTokenService.Guid = Guid.NewGuid();

                    return RedirectToPage("./NewPassword", routeValues: new { Id = _passwordRecoveyTokenService.Guid.ToString() });
                }
                else
                {
                    TempData["Message"] = "کد همخوانی ندارد";
                    return Page();
                }
            }
            return Page();
        }
    }
}
