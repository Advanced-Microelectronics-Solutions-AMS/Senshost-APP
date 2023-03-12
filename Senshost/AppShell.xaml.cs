using Senshost.ViewModels;

namespace Senshost;

public partial class AppShell : Shell
{
    public AppShell(AppShellViewModel appShellViewModel)
    {
        InitializeRouting();
        InitializeComponent();
        BindingContext = appShellViewModel;
    }

    private void InitializeRouting()
    {
        //Routing.RegisterRoute("Filter", typeof(FiltersView));
    }
}
