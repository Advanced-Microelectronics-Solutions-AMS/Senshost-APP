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
    }
}