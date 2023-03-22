using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Senshost.Common.Interfaces;
using Senshost.Models.Notification;
using Senshost.Services;
using Senshost.Views;

namespace Senshost.ViewModels
{
    public partial class AppShellViewModel : BaseObservableViewModel
    {
        private readonly UserStateContext userStateContext;

        public AppShellViewModel(UserStateContext userStateContext)
        {
            Connectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            this.userStateContext=userStateContext;
        }

        [RelayCommand]
        public async void LogOut()
        {
            await userStateContext.LogoutAsync();
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
    }
}
