using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _01_Framework.Application
{
    public class FileExtensionLimitationAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] _validExtensions;
        public FileExtensionLimitationAttribute()
        {
            try
            {
                var json = File.ReadAllText("Settings.json");
                _validExtensions = JsonSerializer.Deserialize<SettingModel>(json).FileExtensionLimit;
            }
            catch
            {

            }
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file == null) return true;
            var fileExtension = Path.GetExtension(file.FileName);
            fileExtension = fileExtension.Remove(0, 1);
            return _validExtensions.Contains(fileExtension);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            //context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-fileExtensionLimit", ErrorMessage);
        }
    }
}
