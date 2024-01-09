using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Senshost_APP.ViewModels;

namespace Senshost_APP.Views;

public partial class NotificationListPage : ContentPage
{
    public NotificationListPage(NotificationListPageViewModel notificationListPageViewModel)
    {
        InitializeComponent();
        BindingContext = notificationListPageViewModel;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Senshost_APP.Platforms.Android.PostNotifications>();

        if (status != PermissionStatus.Granted)
        {
            if (Permissions.ShouldShowRationale<Senshost_APP.Platforms.Android.PostNotifications>())
            {
                await AppShell.Current.DisplayAlert("Allow Notifications", "Please allow App notification permission to receive platform Event Notifications.", "Close");
            }

            status = await Permissions.RequestAsync<Senshost_APP.Platforms.Android.PostNotifications>();

            if (status == PermissionStatus.Denied)
            {
                var toast = Toast.Make("Notification permission request denied by user.", ToastDuration.Short);
                await toast.Show();
            }
        }
    }
}