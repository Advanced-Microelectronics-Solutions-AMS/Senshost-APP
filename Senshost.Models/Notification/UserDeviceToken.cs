using Senshost.Models.Common;

namespace Senshost.Models.Notification
{
    public class UserDeviceToken : BaseModel
    {
        public string AccountId { get; set; }
        public string DeviceRegistrationId { get; set; }
        public string? UserId { get; set; }
        public string DeviceType { get; set; }
        public string UserDeviceId { get; set; }
    }
}
