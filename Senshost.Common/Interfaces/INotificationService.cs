using Senshost.Models.Common;
using Senshost.Models.Notification;

namespace Senshost.Common.Interfaces
{
    public interface INotificationService
    {
        Task<UserDeviceToken> SaveDeviceToken(UserDeviceToken userDeviceToken);
        Task DeleteDeviceToken(string id);
        Task<NotificationCount> GetNotificationCount(string accountId, string userId);
        Task<DataResponse<IEnumerable<NotificationsDetail>>> GetNotifications(string accountId, string userId, int pageSize, int pageNumber, string sortOrder);
        Task<UserNotification> AddUpdateNotificationStatus(UserNotification userNotification);
    }
}
