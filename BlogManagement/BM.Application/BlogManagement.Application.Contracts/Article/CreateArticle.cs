using _01_Framework.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Application.Contracts.Article
{
    public class CreateArticle
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string ShortDescription { get; set; }

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
        public string PublishDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get; set; }

        public string? CanonicalAddress { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public long CategoryId { get; set; }
    }
}
