using Senshost.ViewModels;

namespace Senshost.Views;

public partial class LoadingPage : ContentPage
{
    private readonly UserStateContext userStateContext;

    public LoadingPage(UserStateContext userStateContext)
    {
        InitializeComponent();
        this.userStateContext = userStateContext;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await userStateContext.CheckUserLoginDetails();
    }
}