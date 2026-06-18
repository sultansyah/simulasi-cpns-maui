using Microsoft.Extensions.DependencyInjection;

namespace SimulasiCPNS
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            // force to light theme
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_serviceProvider.GetRequiredService<AppShell>());
        }
    }
}
