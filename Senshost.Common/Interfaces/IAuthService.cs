using Senshost.Models.Account;

namespace Senshost.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> LoginAsync(string username, string password);
    }
}
