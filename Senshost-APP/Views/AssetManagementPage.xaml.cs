using Android.App;
using Android.Webkit;
using Microsoft.Maui.Handlers;
using Senshost_APP.Platforms.Android.CustomRenderers;
using Senshost_APP.ViewModels;
using System.Diagnostics;

namespace Senshost_APP.Views;

public partial class AssetManagementPage : ContentPage
{
    private readonly EventListPageViewModel eventListPageViewModel;
    private bool isFirstLoad = true;

    public AssetManagementPage(EventListPageViewModel eventListPageViewModel)
    {
        InitializeComponent();
        this.eventListPageViewModel = eventListPageViewModel;
        this.BindingContext = eventListPageViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (!isFirstLoad)
            await assetManagementWebView.FadeTo(1, 1000);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        assetManagementWebView.Opacity = 0;
        isFirstLoad = false;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            if (Permissions.ShouldShowRationale<Permissions.Camera>())
            {
                await AppShell.Current.DisplayAlert("Allow Camera Access", "Please allow camera access to enable Asset QR code scaning.", "Ok");
            }

            await Permissions.RequestAsync<Permissions.Camera>();
        }

#if ANDROID
           ((IWebViewHandler)assetManagementWebView.Handler).PlatformView.SetWebChromeClient(new CustomWebChromeClient());
#endif
    }

    protected override bool OnBackButtonPressed()
    {
        base.OnBackButtonPressed();

        if (assetManagementWebView.CanGoBack)
        {
            assetManagementWebView.GoBack();
            return true;
        }
        else
        {
            base.OnBackButtonPressed();
            return true;
        }
    }

    private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        eventListPageViewModel.IsBusy = true;
    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        await assetManagementWebView.EvaluateJavaScriptAsync("setInterval(() => {" +
            "document.getElementsByClassName('header-container')[0]?.style?.setProperty('display', 'none', 'important');" +
            "document.getElementsByClassName('footer-container')[0]?.style?.setProperty('display', 'none', 'important');" +
            "document.getElementsByClassName('main-container')[0]?.style?.setProperty('height', '100vh', 'important');" +
            "}, 100);");

        eventListPageViewModel.IsBusy = false;
        await assetManagementWebView.FadeTo(1, 1000);
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        assetManagementWebView.Reload();
    }
}