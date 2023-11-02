using LampShade.Settings.Domain;
using LampShade.Settings.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Settings
{
    public class IndexModel : PageModel
    {
        private SettingRepository _settingRepository;
        public Setting Setting;

        public IndexModel(SettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public void OnGet()
        {
            Setting = _settingRepository.GetAllSetting();
            if (Setting.FileExtensionLimit is null) Setting.FileExtensionLimit = new string[] { };
        }

        public IActionResult OnPost(Setting setting)
        {
            if (ModelState.IsValid && setting.MaxFileSize >= 1024)
            {
                TempData["success"] = "عملیات با موفیت انجام شد";
                _settingRepository.InsertSetting(setting);
                Setting = setting; ;
                if (Setting.FileExtensionLimit is null) Setting.FileExtensionLimit = new string[] { };
                return RedirectToPage("./Index");
            }
            TempData["Error"] = "مقادیر های اجباری پر نشده اند";
            Setting = setting; ;
            if (Setting.FileExtensionLimit is null) Setting.FileExtensionLimit = new string[] { };
            return Page();
        }
    }
}
