using SimulasiCPNS.Services;

namespace SimulasiCPNS
{
    public partial class AppShell : Shell
    {
        private readonly SettingService _settingService;
        private bool _hasInitializedRoute;

        public AppShell(SettingService settingService)
        {
            _settingService = settingService;
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, EventArgs e)
        {
            if (_hasInitializedRoute)
            {
                return;
            }

            _hasInitializedRoute = true;

            var setting = await _settingService.GetSettingAsync();
            var route = setting is null || string.IsNullOrWhiteSpace(setting.FullName)
                ? "//onboarding"
                : "//home";

            await GoToAsync(route);
        }
    }
}
