using Senshost_APP.Models.Common;
using Senshost_APP.Models.Constants;

namespace Senshost_APP.Models.Notification
{
    public class UserNotification : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
