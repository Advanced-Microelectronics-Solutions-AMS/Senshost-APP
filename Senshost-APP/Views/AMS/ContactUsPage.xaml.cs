using System.Runtime.CompilerServices;

namespace Senshost_APP.Views;

public partial class ContactUsPage : ContentPage
{
    bool isFirstLoad = true;

    public ContactUsPage()
    {
        InitializeComponent();
    }
    protected override bool OnBackButtonPressed()
    {
        base.OnBackButtonPressed();

        if (contactUs.CanGoBack)
        {
            contactUs.GoBack();
            return true;
        }
        else
        {
            base.OnBackButtonPressed();
            return true;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (!isFirstLoad)
            await contactUs.FadeTo(1, 1000);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        contactUs.Opacity = 0;
        isFirstLoad = false;
    }

    private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        loading.IsRunning = true;
        loading.IsVisible = true;
    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        await contactUs.EvaluateJavaScriptAsync("setInterval(() => {" +
           "document.getElementsByClassName('comp-kpmn0qxe')[0]?.style?.setProperty('display', 'none', 'important');" +
           "document.getElementsByClassName('comp-kpnpnjkc')[0]?.style?.setProperty('display', 'none', 'important');" +
           "document.getElementsByClassName('comp-kpnpnfl5')[0]?.style?.setProperty('display', 'none', 'important');" +
           "document.getElementsByClassName('comp-kpnyw74s')[0]?.style?.setProperty('display', 'none', 'important');" +
           "document.getElementsByClassName('comp-kpnsf0o2')[0]?.style?.setProperty('display', 'none', 'important');" +
            "}, 100);");
        loading.IsRunning = false;
        loading.IsVisible = false;
        await contactUs.FadeTo(1, 1500);
    }
}
