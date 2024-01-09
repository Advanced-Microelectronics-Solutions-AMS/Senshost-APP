using Senshost.ViewModels;

namespace Senshost.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel loginPageViewModel)
    {
        InitializeComponent();
        BindingContext = loginPageViewModel;
        Task.Run(AnimateBackground);
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

    private async void AnimateBackground()
    {
        Action<double> forward = input => gridGradient.AnchorY = input;
        Action<double> backward = input => gridGradient.AnchorY = input;

        while (true)
        {
            gridGradient.Animate(name: "forward", callback: forward, start: 0, end: 1, length: 5000, easing: Easing.SinIn);
            await Task.Delay(5000);
            gridGradient.Animate(name: "backward", callback: backward, start: 1, end: 0, length: 5000, easing: Easing.SinIn);
            await Task.Delay(5000);
        }
    }
}