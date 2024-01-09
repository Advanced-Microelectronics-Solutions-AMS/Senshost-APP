using CommunityToolkit.Mvvm.ComponentModel;
using Senshost_APP.Models.Constants;
using Senshost_APP.Models.Notification;

namespace Senshost_APP.ViewModels
{
    [QueryProperty(nameof(Notification), nameof(Notification))]
    public partial class NotificationDetailPageViewModel : BaseObservableViewModel
    {
        public NotificationDetailPageViewModel()
        {
            Notification = new Notification()
            {
                CreationDate = DateTime.UtcNow
            };
        }

        [ObservableProperty]
        private Notification notification;

        [ObservableProperty]
        private NotificationStatus? status;
        public string UserNotificationId { get; set; }

        partial void OnNotificationChanged(Notification oldValue, Notification newValue)
        {

        }
    }
}
