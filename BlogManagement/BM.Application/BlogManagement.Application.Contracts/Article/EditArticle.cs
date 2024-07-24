using _01_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Application.Contracts.Article
{
    public class EditArticle : CreateArticle
    {
        public long Id { get; set; }

        [MaxFileSize(ErrorMessage = ValidationMessages.MaxFileSize)]
        [FileExtensionLimitation(ErrorMessage = ValidationMessages.InvalidFileFormat)]
        public new IFormFile? Picture { get; set; }
    }
}
