using Senshost.Common.Interfaces;
using Senshost.Constants;
using Senshost.Models.Common;
using Senshost.Models.Constants;
using Senshost.Models.Notification;
using Senshost.Services.Common;

namespace Senshost.Services
{
    public class NotificationService : BaseHttpService, INotificationService
    {
        public NotificationService(HttpClient httpClient, IStorageService storageService) : base(httpClient)
        {
        }

        public async Task DeleteDeviceToken(string id)
        {
            await DeleteAsync(APIConstants.DeleteUserDeviceTokenUrl(id));
        }

        public async Task<UserDeviceToken> SaveDeviceToken(UserDeviceToken userDeviceToken)
        {
            return await PostAsync<UserDeviceToken>(APIConstants.SaveUserDeviceTokenUrl, userDeviceToken);
        }

        public async Task<NotificationCount> GetNotificationCount(string accountId, string userId)
        {
            var notificationCount = await GetAsync<NotificationCount>(APIConstants.GetNotificationsCountUrl(accountId.ToString(), userId?.ToString()));
            return notificationCount;
        }

        public async Task<DataResponse<IEnumerable<NotificationsDetail>>> GetNotifications(string accountId, string userId,
            SeverityLevel? severityLevel, NotificationStatus? notificationStatus,
            int pageSize, int pageNumber, string sortOrder)
        {
            return await GetAsync<DataResponse<IEnumerable<NotificationsDetail>>>(
                APIConstants.GetNotifications(accountId.ToString(), userId?.ToString(), severityLevel, notificationStatus, pageSize, pageNumber, sortOrder));
        }

        public async Task<UserNotification> AddUpdateNotificationStatus(UserNotification userNotification)
        {
            return await PostAsync<UserNotification>(APIConstants.AddUpdateNotificationStatusUrl(), userNotification);
        }
    }
}
