using _01_Framework.Application;
using Microsoft.AspNetCore.Http;
using ShopManagement.Application.Contracts.Product;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Application.Contracts.ProductPicture
{
    public class CreateProductPicture
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxFileSize(ErrorMessage = ValidationMessages.MaxFileSize)]
        [FileExtensionLimitation(ErrorMessage = ValidationMessages.InvalidFileFormat)]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PicutreAlt { get; set; }

        [Range(1, 100000, ErrorMessage = ValidationMessages.IsRequired)]
        public long ProductId { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
