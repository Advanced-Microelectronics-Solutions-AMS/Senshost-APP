namespace Senshost.Common.Constants
{
    public static class Constants
    {
        public const string ApiSecureStorageToken = "api-token";
        public static string LoginUrl => "api/auth/login";
        public static string SaveUserDeviceTokenUrl => "api/notification/device/token";
        public static string DeleteUserDeviceTokenUrl(string id) => $"api/notification/device/token/{id}";
        public static string GetNotificationsCountUrl(string accountId, string userId) => $"api/notification/account/{accountId}/count?userId={userId}";
        public static string GetNotifications(string accountId, string userId, int pageSize, int pageNumber, string sortOrder) => 
                    $"api/notification/account/{accountId}?userId={userId}&PageSize={pageSize}&PageNumber={pageNumber}&Sort={sortOrder}";
        public static string AddUpdateNotificationStatusUrl() => $"api/notification/user/notification";
    }
}
