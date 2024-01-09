using Senshost_APP.Models.Common;
using Senshost_APP.Models.Constants;

namespace Senshost_APP.Models.Notification
{
    public class Notification : BaseModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Recipients { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public string Priority { get; set; }
        public RecipientType RecipientType { get; set; }
        public SeverityLevel Severity { get; set; }
        public Guid? AccountId { get; set; }
    }

    public class NotificationsDetail : Notification
    {
        public NotificationStatus Status { get; set; }
        public string UserNotificationId { get; set; }

    }

    public class NotificationCount
    {
        public int Total { get; set; }
        public int TotalPending { get; set; }
        public int Info { get; set; }
        public int Warning { get; set; }
        public int Critical { get; set; }
    }

    public class NotificationTapped
    {
        public Notification Notification { get; set; }
    }
}
