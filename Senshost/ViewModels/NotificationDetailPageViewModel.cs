using CommunityToolkit.Mvvm.ComponentModel;
using Senshost.Models.Constants;
using Senshost.Models.Notification;

namespace Senshost.ViewModels
{
    public partial class NotificationDetailPageViewModel : BaseObservableViewModel
    {
        public Notification Notification { get; set; } = new();

        [ObservableProperty]
        public NotificationStatus? status;
        public string UserNotificationId { get; set; }
    }
}
