using _01_Framework.Application;
using _01_Framework.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Application.Contracts.Role
{
    public class CreateRole
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }
        public List<int> Permissions { get; set; }
    }
}
