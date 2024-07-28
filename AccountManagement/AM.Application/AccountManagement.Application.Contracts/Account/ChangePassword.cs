using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Application.Contracts.Account
{
    public class ChangePassword
    {
        public long Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(4, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MinLenght)]
        public string Password { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(4, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(255, ErrorMessage = ValidationMessages.MinLenght)]
        [Compare("Password", ErrorMessage = "رمز ورود با تکرار آن همخوانی ندارد")]
        public string RePassword { get; set; }
    }
}
