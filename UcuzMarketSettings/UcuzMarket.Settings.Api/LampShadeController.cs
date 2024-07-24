using LampShade.Settings.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LampShade.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class LampShadeController : ControllerBase
    {
        private readonly SettingRepository _settingRepository;

        public LampShadeController(SettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        [HttpGet("GetMaxFileSizeLimit")]
        public int GetMaxFileSizeLimit()
        {
            return _settingRepository.GetAllSetting().MaxFileSize;
        }

        [HttpGet("GetFileExtensionLimit")]
        public string[] GetFileExtensionLimit()
        {
            return _settingRepository.GetAllSetting().FileExtensionLimit;
        }
    }
}
