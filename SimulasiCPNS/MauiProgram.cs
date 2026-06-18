using Microsoft.Extensions.Logging;
using SimulasiCPNS.Services;
using SimulasiCPNS.Views;

namespace SimulasiCPNS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SettingService>();

            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<OnboardingPage>();
            builder.Services.AddTransient<HomePage>();
#endif

            return builder.Build();
        }
    }
}
