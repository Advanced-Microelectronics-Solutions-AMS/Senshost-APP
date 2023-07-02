using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Senshost.ViewModels
{
    public partial class AppShellViewModel : BaseObservableViewModel
    {
        private readonly UserStateContext userStateContext;

        [ObservableProperty]
        string badgeCount;

        public AppShellViewModel(UserStateContext userStateContext)
        {
            Connectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            userStateContext.PropertyChanged += OnUserStatePropertyChanged;
            BadgeCount = userStateContext.BadgeCount;
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

        private void OnUserStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(userStateContext.BadgeCount))
            {
                BadgeCount = userStateContext.BadgeCount;
            }
        }
    }
}
