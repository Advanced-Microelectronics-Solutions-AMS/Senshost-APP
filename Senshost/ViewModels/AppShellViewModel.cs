using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Senshost.Common.Interfaces;
using Senshost.Services;

namespace Senshost.ViewModels
{
    public partial class AppShellViewModel : BaseObservableViewModel
    {
        private readonly UserStateContext userStateContext;
        private readonly INotificationService notificationService;

        public AppShellViewModel(UserStateContext userStateContext, INotificationService notificationService)
        {
            this.userStateContext = userStateContext;
            this.notificationService=notificationService;
            userStateContext.PropertyChanged +=UserStateContext_PropertyChanged;
            Connectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        [ObservableProperty]
        private int notificationCount;

        [RelayCommand]
        public async void LogOut()
        {
            await userStateContext.LogoutAsync();
        }

        public void GetNotificationCount()
        {
            Task.Run(async () =>
            {
                var result = await notificationService.GetNotificationCount(App.UserDetails.AccountId, App.UserDetails.UserId);
                NotificationCount = result.TotalPending;
            });
        }

        private async void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet || e.NetworkAccess == NetworkAccess.None)
            {
                var toast = Toast.Make("Connection lost!", ToastDuration.Long);
                await toast.Show();
            }
            else if (e.NetworkAccess == NetworkAccess.Internet)
            {
                var toast = Toast.Make("Back online", ToastDuration.Long);
                await toast.Show();
            }
        }

        private void UserStateContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(userStateContext.IsAuthorized))
            {
                if (userStateContext.IsAuthorized)
                {
                    //GetNotificationCount();
                }
            }
        }
    }
}
