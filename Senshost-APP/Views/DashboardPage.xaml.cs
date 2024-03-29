using Senshost_APP.ViewModels;

namespace Senshost_APP.Views;

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
            dashboard.Source = "https://senshost.com/#/home/dashboard";
            isFirstLoad = false;
        }
        else
            await dashboard.FadeTo(1, 1000);
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
                "}, 100);");

        dashboardPageViewModel.IsBusy = false;
        await dashboard.FadeTo(1, 1000);
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