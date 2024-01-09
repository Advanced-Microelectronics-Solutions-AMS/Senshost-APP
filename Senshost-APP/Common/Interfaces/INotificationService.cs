using Senshost_APP.Models.Common;
using Senshost_APP.Models.Constants;
using Senshost_APP.Models.Notification;

namespace Senshost_APP.Common.Interfaces
{
    public interface INotificationService
    {
        Task<UserDeviceToken> SaveDeviceToken(UserDeviceToken userDeviceToken);
        Task DeleteDeviceToken(string id);
        Task<NotificationCount> GetNotificationCount(string accountId, string userId);
        Task<DataResponse<IEnumerable<NotificationsDetail>>> GetNotifications(string accountId, string userId,
            SeverityLevel? severityLevel, NotificationStatus? notificationStatus, int? pageSize, int? pageNumber, string sortOrder);
        Task<UserNotification> AddUpdateNotificationStatus(UserNotification userNotification);
    }
}
