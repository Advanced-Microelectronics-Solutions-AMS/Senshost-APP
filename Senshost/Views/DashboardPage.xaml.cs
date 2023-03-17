using Const = Senshost.Common.Constants;
using Senshost.Common.Interfaces;
using Senshost.ViewModels;

namespace Senshost.Views;

public partial class DashboardPage : ContentPage
{
    private bool isFirstLoad = true;
    private bool isReload = false;

    private readonly DashboardPageViewModel dashboardPageViewModel;

    public DashboardPage(DashboardPageViewModel dashboardPageViewModel)
    {
        InitializeComponent();

        dashboardPageViewModel.UserLoggedOut += ClearLocalStorage;
        this.dashboardPageViewModel = dashboardPageViewModel;
        BindingContext = dashboardPageViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (isFirstLoad)
        {
            dashboard.Source = "https://bat-services.netlify.app/#/home/dashboard";
            isFirstLoad = false;
        }
        else
            await dashboard.FadeTo(1, 1000);

        dashboardPageViewModel.BadgeCount = Senshost.App.BadgeCount;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        dashboard.Opacity = 0;
    }

    private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
        dashboardPageViewModel.IsBusy = true;
    }

    protected override bool OnBackButtonPressed()
    {

        base.OnBackButtonPressed();

        if (dashboard.CanGoBack)
        {
            dashboard.GoBack();
            return true;
        }
        else
        {
            base.OnBackButtonPressed();
            return true;
        }

    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
        if (!isReload)
        {
            var ls = "'{\"auth\":\"{\\\\\"isFetching\\\\\":false,\\\\\"auth\\\\\":{\\\\\"identityToken\\\\\":\\\\\"" +
                $"{App.ApiToken}" +
                "\\\\\",\\\\\"account\\\\\":{\\\\\"name\\\\\":\\\\\"" +
                $"{App.UserDetails?.Name}" +
                "\\\\\",\\\\\"email\\\\\":\\\\\"" +
                $"{App.UserDetails?.Email}" +
                "\\\\\",\\\\\"username\\\\\":\\\\\"" +
                $"{App.UserDetails?.Name}" +
                "\\\\\",\\\\\"id\\\\\":\\\\\"" +
                $"{App.UserDetails?.AccountId}" +
                "\\\\\",\\\\\"password\\\\\":\\\\\"" +
                $"{App.UserDetails?.Password}" +
                "\\\\\"},\\\\\"group\\\\\":";

            if (string.IsNullOrEmpty(App.UserDetails?.GroupId))
                ls += "null";
            else
            {
                ls += "{\\\\\"accountId\\\\\":\\\\\"" +
                $"{App.UserDetails?.AccountId}" +
                "\\\\\",\\\\\"name\\\\\":\\\\\"" +
                $"{App.UserDetails?.GroupName}" +
                "\\\\\",\\\\\"status\\\\\":" +
                $"{(int)App.UserDetails?.GroupStatus}" +
                ",\\\\\"id\\\\\":\\\\\"" +
                $"{App.UserDetails?.GroupId}" +
                "\\\\\",\\\\\"creationDate\\\\\":\\\\\"2022-03-05T05:17:55.954877\\\\\"}";
            }
            ls += "},\\\\\"error\\\\\":null,\\\\\"isAuthenticated\\\\\":true}\",\"settings\":\"{\\\\\"dashboardRefresh\\\\\":60000}\",\"pageSize\":\"{\\\\\"eventSize\\\\\":10,\\\\\"dataValueSize\\\\\":10,\\\\\"assetLandingPageRowSize\\\\\":10,\\\\\"isFetching\\\\\":false,\\\\\"error\\\\\":null}\",\"_persist\":\"{\\\\\"version\\\\\":-1,\\\\\"rehydrated\\\\\":true}\"}'";

            var js = $"localStorage.setItem('persist:senhost',{ls})";
            await dashboard.EvaluateJavaScriptAsync(js);
            isReload = true;
            dashboard.Reload();

            return;
        }

        await dashboard.EvaluateJavaScriptAsync("setInterval(() => {" +
                "document.getElementsByClassName('header-container')[0]?.style?.setProperty('display', 'none', 'important');" +
                "document.getElementsByClassName('footer-container')[0]?.style?.setProperty('display', 'none', 'important');" +
                "document.getElementsByClassName('main-container')[0]?.style?.setProperty('height', '100vh', 'important');" +
                "document.getElementsByClassName('dashboard-container')[0]?.childNodes[0]?.style.setProperty('display', 'none', 'important');" +
                "document.getElementsByClassName('dashboard-container')[0]?.childNodes[1]?.style.setProperty('display', 'none', 'important');" +
                "document.getElementsByName('search')[0]?.style?.setProperty('display', 'none', 'important');" +
                "document.getElementsByClassName('left-side-container')[0]?.style?.setProperty('display', 'none', 'important');" +
                "document.getElementsByClassName('dashboards-holder')[0]?.style?.setProperty('margin', '0px', 'important');" +
                "document.getElementsByClassName('dashboards-holder')[0]?.style?.setProperty('padding', '0px 10px', 'important');" +
                "for (const s of document.getElementsByClassName('dashboard-card')) {s?.style?.setProperty('flex', '0 0 100%', 'important'); " +
                "s?.style?.setProperty('max-width', '100%', 'important');" +
                "s?.style?.setProperty('padding', '0px', 'important');" +
                "s?.style?.setProperty('margin-right', '0px', 'important');" +
                "s?.style?.setProperty('margin-left', '0px', 'important');};" +
                "}, 100);");

        dashboardPageViewModel.IsBusy = false;
        await dashboard.FadeTo(1, 2000);
    }

    async void ClearLocalStorage(object sender, EventArgs e)
    {
        await dashboard.EvaluateJavaScriptAsync("localStorage.removeItem('persist:senhost')");
        isFirstLoad = true;
        isReload = false;
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        dashboard.Opacity = 0;
        isReload = true;
        dashboard.Reload();
    }
}