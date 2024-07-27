using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DeliveryManagement.Application.Contract.Delivery
{
    public class CreateDelivery
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(10, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(512, ErrorMessage = ValidationMessages.MaxLenght)]
        public string Address { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MinLength(10, ErrorMessage = ValidationMessages.MinLenght)]
        [MaxLength(10, ErrorMessage = ValidationMessages.MaxLenght)]
        public string PostalCode { get; set; }

        [NotNull]
        public long AccountId { get; set; }
    }
}
