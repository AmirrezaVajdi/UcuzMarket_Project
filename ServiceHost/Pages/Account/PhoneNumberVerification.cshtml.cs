using _0_Framework.Application.Sms;
using _01_Framework.Application;
using _01_Framework.Application.Sms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ServiceHost.Pages.Account
{
    public class PhoneNumberVerificationModel : PageModel
    {
        private PasswordRecoveyTokenService _passwordRecoveyTokenService;
        private readonly VerficationCodeService _verficationCodeService;
        private readonly ISmsService _smsService;

        public PasswordRecoveryDto PasswordRecoveryDto { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(6, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(6, ErrorMessage = ValidationMessages.MinLenght)]
        [BindProperty]
        public string Code { get; set; }

        [BindProperty]
        public Guid Guid { get; set; }


        public PhoneNumberVerificationModel(PasswordRecoveyTokenService passwordRecoveyTokenService, VerficationCodeService verficationCodeService, ISmsService smsService)
        {
            _passwordRecoveyTokenService = passwordRecoveyTokenService;
            _verficationCodeService = verficationCodeService;
            _smsService = smsService;
        }

        public async Task<IActionResult> OnGet([FromQuery] Guid Id)
        {
            PasswordRecoveryDto = _passwordRecoveyTokenService.GetBy(Id);

            if (Id != Guid.Empty && PasswordRecoveryDto != null)
            {
                var result = await _smsService.SendVerificationCodeAsync(PasswordRecoveryDto.PhoneNumber);
                if (!result)
                {
                    TempData["Message"] = "مشکلی در ارسال پیامک بوجود امد لطفا بعدا امتحان نمایید";
                }
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

                var verifyModel = _verficationCodeService.GetVerificationBy(PasswordRecoveryDto.PhoneNumber);

                if (verifyModel.verifyCode == int.Parse(Code))
                {
                    _verficationCodeService.Done(PasswordRecoveryDto.PhoneNumber);

                    return RedirectToPage("./NewPassword", routeValues: new { Id = PasswordRecoveryDto.Guid.ToString() });
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
