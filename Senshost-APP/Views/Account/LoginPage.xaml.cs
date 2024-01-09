using Senshost_APP.ViewModels;

namespace Senshost_APP.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel loginPageViewModel)
    {
        InitializeComponent();
        BindingContext = loginPageViewModel;

        LoginPanel.Dispatcher.Dispatch(() => LoginPanel.Animate("FadeIn", FadeIn(), 16, Convert.ToUInt32(1000)));
    }

    internal Animation FadeIn()
    {
        var animation = new Animation();

        animation.WithConcurrent((f) => LoginPanel.Opacity = f, 0, 1, Microsoft.Maui.Easing.CubicOut);

        animation.WithConcurrent(
          (f) => LoginPanel.TranslationY = f,
          LoginPanel.TranslationY + 50, LoginPanel.TranslationY,
          Microsoft.Maui.Easing.CubicOut, 0, 1);

        return animation;
    }
}