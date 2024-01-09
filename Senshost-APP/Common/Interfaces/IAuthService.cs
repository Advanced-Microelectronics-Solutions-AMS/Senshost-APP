using Senshost_APP.Models.Account;

namespace Senshost_APP.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> LoginAsync(string username, string password);
    }
}
