using Microsoft.Extensions.DependencyInjection;

namespace SimulasiCPNS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // force to light theme
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}