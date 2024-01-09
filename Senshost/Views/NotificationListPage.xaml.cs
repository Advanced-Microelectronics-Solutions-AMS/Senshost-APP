using Senshost.ViewModels;

namespace Senshost.Views;

public partial class NotificationListPage : ContentPage
{
    private readonly NotificationListPageViewModel vm;

    public NotificationListPage(NotificationListPageViewModel notificationListPageViewModel)
    {
        InitializeComponent();
        this.vm = notificationListPageViewModel;
        BindingContext = notificationListPageViewModel;
    }
}