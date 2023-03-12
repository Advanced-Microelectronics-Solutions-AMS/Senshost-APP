namespace Senshost.Models.Constants
{
    public enum UserRole
    {
        View,
        Edit,
        Admin,
        Su
    }

    public enum GroupStatus
    {
        Active,
        InActive
    }

    public enum DataSorting
    {
        Ascending, 
        Descending
    }

    public enum RecipientType
    {
        All,
        Group,
        User,
        AccountOwner
    }

    public enum SeverityLevel
    {
        Info,
        Warning,
        Critical
    }

    public enum NotificationStatus
    {
        Pending,
        Read,
        Deleted
    }
}
