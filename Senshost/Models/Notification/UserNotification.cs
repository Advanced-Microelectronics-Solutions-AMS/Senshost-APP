using Senshost.Models.Common;
using Senshost.Models.Constants;

namespace Senshost.Models.Notification
{
    public class UserNotification : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid NotificationId { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
