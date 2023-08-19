using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace CommandManagement.Application.Contract.Comment
{
    public class AddComment
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Email { get; set; }
        public string? WebSite { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Message { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public long OwnerRecordId { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public int Type { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public long ParentId { get; set; }

    }
}
