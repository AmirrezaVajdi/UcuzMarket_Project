using LampShade.Settings.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace LampShade.Settings
{
    public static class LampShadeSettingsBootstrapper
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddTransient<SettingRepository>();
        }
    }
}
