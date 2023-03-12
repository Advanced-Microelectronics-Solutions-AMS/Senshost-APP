using Senshost.Models.Constants;

namespace Senshost.Models.Account
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
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string AccountId { get; set; }
        public string Name { get; set; }
        public GroupStatus Status { get; set; }
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
}
