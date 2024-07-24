using _01_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Application.Contracts.ArticleCategory
{
    public class CreateArticleCategory
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        [MaxFileSize(ErrorMessage = ValidationMessages.MaxFileSize)]
        [FileExtensionLimitation(ErrorMessage = ValidationMessages.InvalidFileFormat)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureAlt { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Description { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public int ShowOrder { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Metadescription { get; set; }
        public string? CanonicalAddress { get; set; }

    }
}
