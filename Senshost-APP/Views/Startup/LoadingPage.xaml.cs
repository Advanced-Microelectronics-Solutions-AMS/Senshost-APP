namespace Senshost_APP.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
        LogoImage.Opacity = 0;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = LogoImage.FadeTo(1, 500, Easing.SinIn);
    }
}