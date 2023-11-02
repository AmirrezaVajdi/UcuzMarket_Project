using LampShade.Settings.Domain;
using System.Text.Json;

namespace LampShade.Settings.Repository
{
    public class SettingRepository
    {
        private const string _fileName = "Settings.json";

        public Setting GetAllSetting()
        {
            try
            {
                var json = File.ReadAllText(_fileName);
                return JsonSerializer.Deserialize<Setting>(json);
            }
            catch
            {
                return new();
            }

        }
        public void InsertSetting(Setting model)
        {
            try
            {
                var res = JsonSerializer.Serialize<Setting>(model);
                File.WriteAllText(_fileName, res);
            }
            catch
            {
            }
        }

        public void UpdateSetting(Setting model)
        {
            DeleteSetting(model);
        }

        public void DeleteSetting(Setting model)
        {
            try
            {
                File.Delete(_fileName);
                InsertSetting(model);
            }
            catch
            {

            }
        }
    }
}
