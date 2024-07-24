using _01_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Application.Contracts.ProdcutCategory
{
    public class CreateProductCategory
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Description { get; set; }

        [MaxFileSize(ErrorMessage = ValidationMessages.MaxFileSize)]
        [FileExtensionLimitation(ErrorMessage = ValidationMessages.InvalidFileFormat)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureAlt { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string KeyWords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }
        public long? ParentId { get; set; }
        public List<ProductCategoryViewModel> Categoires { get; set; }
    }
}
