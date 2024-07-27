using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Application.Contracts.Account
{
    public class Login
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(11, ErrorMessage = ValidationMessages.MaxLenght)]
        [MinLength(11, ErrorMessage = ValidationMessages.MinLenght)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(4, ErrorMessage = ValidationMessages.MinLenght)]
        public string Password { get; set; }
    }
}
