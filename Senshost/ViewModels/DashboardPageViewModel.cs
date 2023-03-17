using System.ComponentModel;

namespace Senshost.ViewModels
{
    public partial class DashboardPageViewModel : BaseObservableRecipientViewModel
    {
        private readonly UserStateContext userStateContext;

        public event EventHandler UserLoggedOut;

        public DashboardPageViewModel(UserStateContext userStateContext)
        {
            userStateContext.PropertyChanged += OnUserStatePropertyChanged;
            this.userStateContext = userStateContext;

            GetNotificationCount();
        }

        private void OnUserStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(userStateContext.IsAuthorized))
            {
                if (!userStateContext.IsAuthorized)
                {
                    UserLoggedOut?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private async void GetNotificationCount()
        {
            var notificationCount = await userStateContext.GetNotificationCount();
            BadgeCount = "" + notificationCount.TotalPending;
            Senshost.App.BadgeCount = BadgeCount;
        }
    }
}
