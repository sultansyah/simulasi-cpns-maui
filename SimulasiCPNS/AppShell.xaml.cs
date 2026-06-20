using SimulasiCPNS.Services;
using SimulasiCPNS.Views;

namespace SimulasiCPNS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(QuestionPage), typeof(QuestionPage));
        }
    }
}
