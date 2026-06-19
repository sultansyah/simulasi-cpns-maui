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

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SettingService>();
            builder.Services.AddSingleton<QuestionService>();
            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<OnboardingPage>();
            builder.Services.AddTransient<HomePage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
