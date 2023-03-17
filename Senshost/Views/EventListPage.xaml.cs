using Senshost.ViewModels;

namespace Senshost.Views;

public partial class EventListPage : ContentPage
{
    private readonly EventListPageViewModel eventListPageViewModel;
    private bool isFirstLoad = true;

    public EventListPage(EventListPageViewModel eventListPageViewModel)
    {
        InitializeComponent();
        this.eventListPageViewModel = eventListPageViewModel;
        this.BindingContext = eventListPageViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (!isFirstLoad)
            await events.FadeTo(1, 1000);

        eventListPageViewModel.BadgeCount = Senshost.App.BadgeCount;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        events.Opacity = 0;
        isFirstLoad = false;

    }

    protected override bool OnBackButtonPressed()
    {
        base.OnBackButtonPressed();

        if (events.CanGoBack)
        {
            events.GoBack();
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
        await events.EvaluateJavaScriptAsync("setInterval(() => {" +
            "document.getElementsByClassName('header-container')[0]?.style?.setProperty('display', 'none', 'important');" +
            "document.getElementsByClassName('footer-container')[0]?.style?.setProperty('display', 'none', 'important');" +
            "document.getElementsByClassName('left-side-container')[0]?.style?.setProperty('display', 'none', 'important');" +
            "document.getElementsByClassName('main-container')[0]?.style?.setProperty('height', '100vh', 'important');" +
            "document.querySelectorAll('.main-holder .container-fluid .card-header')[0]?.style?.setProperty('display', 'none', 'important');" +
            "}, 100);");

        eventListPageViewModel.IsBusy = false;
        await events.FadeTo(1, 1500);
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        events.Reload();
    }
}