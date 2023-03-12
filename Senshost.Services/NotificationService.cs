using Senshost.Common.Constants;
using Senshost.Common.Interfaces;
using Senshost.Models.Common;
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
            await DeleteAsync(Constants.DeleteUserDeviceTokenUrl(id));
        }

        public async Task<UserDeviceToken> SaveDeviceToken(UserDeviceToken userDeviceToken)
        {
            return await PostAsync<UserDeviceToken>(Constants.SaveUserDeviceTokenUrl, userDeviceToken);
        }

        public async Task<NotificationCount> GetNotificationCount(string accountId, string userId)
        {
            return await GetAsync<NotificationCount>(Constants.GetNotificationsCountUrl(accountId.ToString(), userId?.ToString()));
        }

        public async Task<DataResponse<IEnumerable<NotificationsDetail>>> GetNotifications(string accountId, string userId, int pageSize, int pageNumber, string sortOrder)
        {
            return await GetAsync<DataResponse<IEnumerable<NotificationsDetail>>>(
                Constants.GetNotifications(accountId.ToString(), userId?.ToString(), pageSize, pageNumber, sortOrder));
        }

        public async Task<UserNotification> AddUpdateNotificationStatus(UserNotification userNotification)
        {
            return await PostAsync<UserNotification>(Constants.AddUpdateNotificationStatusUrl(), userNotification);
        }
    }
}
