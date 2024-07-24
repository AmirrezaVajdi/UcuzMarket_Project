using _01_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Application.Contracts.Slide
{
    public class EditSlide : CreateSlide
    {
        public long Id { get; set; }

        [MaxFileSize(ErrorMessage = ValidationMessages.MaxFileSize)]
        [FileExtensionLimitation(ErrorMessage = ValidationMessages.InvalidFileFormat)]
        public new IFormFile? Picture { get; set; }
    }
}
