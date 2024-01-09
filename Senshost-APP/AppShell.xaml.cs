using Senshost_APP.ViewModels;
using Senshost_APP.Views;

namespace Senshost_APP
{
    public partial class AppShell : Shell
    {
        public AppShell(UserStateContext appShellViewModel)
        {
            InitializeComponent();
            BindingContext = appShellViewModel;
            Routing.RegisterRoute(nameof(NotificationDetailPage), typeof(NotificationDetailPage));
        }
    }
}