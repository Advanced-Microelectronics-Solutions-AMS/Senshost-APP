using Senshost_APP.Models.Common;
using Senshost_APP.Models.Constants;

namespace Senshost_APP.Models.Account
{
    public class AuthenticationResponse
    {
        public Account Account { get; set; }
        public AuthResult AuthResult { get; set; }
        public string IdentityToken { get; set; }
    }

    public class AuthResult
    {
        public User User { get; set; }
        public Group Group { get; set; }
    }

    public class Account
    {
        public DateTime? CreationDate { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string GroupId { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public List<UserPermission> Permissions { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public GroupStatus Status { get; set; }
        public List<GroupPermission> Permissions { get; set; }

    }

    public class LogedInUserDetails
    {
        public string AccountId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserRole? UserRole { get; set; }
        public string GroupName { get; set; }
        public GroupStatus? GroupStatus { get; set; }
        public string GroupId { get; set; }
    }

    public class UserPermission
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public Guid PermissionId { get; set; }
        public User User { get; set; }
        public Permission Permission { get; set; }
    }

    public class Permission
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class GroupPermission : BaseModel
    {
        public Guid AccountId { get; set; }
        public Guid GroupId { get; set; }
        public Guid PermissionId { get; set; }
        public Group Group { get; set; }
        public Permission Permission { get; set; }
    }
}
