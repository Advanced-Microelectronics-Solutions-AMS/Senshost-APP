using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using Senshost.Constants;
using Senshost.Models.Notification;

namespace Senshost.Views;

[QueryProperty(nameof(Notification), "Notification")]
public partial class NotificationDetailPage : ContentPage
{
    public Notification Notification
    {
        set
        {
            BindingContext = value;
        }
    }

    public NotificationDetailPage(Notification notification)
    {
        InitializeComponent();
        Notification = notification;
        string color = GetColor(notification);

        topTitleBar.BackgroundColor = Color.FromArgb(color);
        lblType.BackgroundColor = Color.FromArgb(color);
        borderNotification.BackgroundColor = Color.FromArgb(color);
    }

    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var notification = BindingContext as Notification;
        string color = GetColor(notification);

        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb(color),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#if ANDROID
        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Window.SetNavigationBarColor(Android.Graphics.Color.ParseColor(color));
#endif
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        this.Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Color.FromArgb(AppConstants.primaryColor),
            StatusBarStyle = StatusBarStyle.LightContent
        });
#if ANDROID
        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.Window.SetNavigationBarColor(Android.Graphics.Color.ParseColor(AppConstants.primaryColor));
#endif
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private static string GetColor(Notification notification)
    {
        string color = AppConstants.InfoNotificationColor;
        if (notification.Severity == Models.Constants.SeverityLevel.Warning)
            color = AppConstants.WarningNotificationColor;
        else if (notification.Severity == Models.Constants.SeverityLevel.Critical)
            color = AppConstants.CriticalNotificationColor;
        return color;
    }
}