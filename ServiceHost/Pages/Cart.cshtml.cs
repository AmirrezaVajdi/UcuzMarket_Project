using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using _01_Query.Contract.Product;
using ShopManagement.Application.Contracts.Order;
using LampShade.Settings.Repository;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        private SettingRepository _settingRepository;
        public double DeliveryFee { get; set; }

        public CartModel(SettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public void OnGet()
        {
            DeliveryFee = _settingRepository.GetAllSetting().DeliveryFee;
        }
    }
}
