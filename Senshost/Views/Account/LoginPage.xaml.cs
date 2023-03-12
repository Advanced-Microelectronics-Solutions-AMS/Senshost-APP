using Senshost.ViewModels;

namespace Senshost.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginPageViewModel loginPageViewModel;

    public LoginPage(LoginPageViewModel loginPageViewModel)
    {
        InitializeComponent();
        this.loginPageViewModel = loginPageViewModel;
        BindingContext = loginPageViewModel;
    }

    private void BorderlessEntry_Completed(object sender, EventArgs e)
    {
        password.Focus();
    }

    private void password_Completed(object sender, EventArgs e)
    {
        password.IsEnabled = false;
        password.IsEnabled = true;
    }
}