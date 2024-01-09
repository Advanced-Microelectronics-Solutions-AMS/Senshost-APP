using Senshost_APP.Constants;
using Senshost_APP.Models.Notification;

namespace Senshost_APP.Views;

public partial class NotificationDetailPage : ContentPage, IQueryAttributable
{
    public NotificationDetailPage()
    {
        InitializeComponent();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var notification = query["Notification"] as Notification;
        BindingContext = notification;
        string color = GetColor(notification);
        lblType.BackgroundColor = Color.FromArgb(color);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
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