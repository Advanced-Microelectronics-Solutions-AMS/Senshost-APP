using Senshost.ViewModels;

namespace Senshost;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel appShellViewModel)
    {
        InitializeComponent();
        BindingContext = appShellViewModel;
    }
}
