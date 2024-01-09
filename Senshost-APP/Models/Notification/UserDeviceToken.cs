using Senshost_APP.Models.Common;

namespace Senshost_APP.Models.Notification
{
    public class UserDeviceToken : BaseModel
    {
        public string AccountId { get; set; }
        public string DeviceRegistrationId { get; set; }
        public string UserId { get; set; }
        public string DeviceType { get; set; }
        public string UserDeviceId { get; set; }
    }
}
