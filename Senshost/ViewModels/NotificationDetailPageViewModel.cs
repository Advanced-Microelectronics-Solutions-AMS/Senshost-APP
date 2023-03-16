using CommunityToolkit.Mvvm.ComponentModel;
using Senshost.Common.Interfaces;
using Senshost.Models.Constants;
using Senshost.Models.Notification;
using Senshost.Services;

namespace Senshost.ViewModels
{
    public partial class NotificationDetailPageViewModel : BaseObservableViewModel
    {
        public Notification Notification { get; set; }

        [ObservableProperty]
        public NotificationStatus? status;
        public string UserNotificationId { get; set; }
        private readonly INotificationService notificationService;


        public NotificationDetailPageViewModel()
        {
            //this.notificationService = notificationService;

            //UpdateNotificationStatus();
        }

        public void UpdateNotificationStatus()
        {            

            if (Status == null)
            {
                Status = Models.Constants.NotificationStatus.Read;

                _ = Task.Run(async () =>
                {
                    await notificationService.AddUpdateNotificationStatus(new UserNotification()
                    {
                        UserId = new Guid(App.UserDetails.UserId ?? App.UserDetails.AccountId),
                        NotificationId = Notification.Id.Value,
                        Status = Models.Constants.NotificationStatus.Read
                    });
                });
            }
        }
    }
}
