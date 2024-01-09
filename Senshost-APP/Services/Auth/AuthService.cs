using Senshost_APP.Common.Interfaces;
using Senshost_APP.Constants;
using Senshost_APP.Models.Account;
using Senshost_APP.Services.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Senshost_APP.Services.Auth
{
    public class AuthService : BaseHttpService, IAuthService
    {
        private readonly HttpClient httpClient;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public AuthService(HttpClient httpClient) : base(httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<AuthenticationResponse> LoginAsync(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, APIConstants.LoginUrl);
            request.Headers.Add("CallerType", "Mobile");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            var res = await httpClient.SendAsync(request);
            if (res.IsSuccessStatusCode)
            {

                var content = await res.Content.ReadAsStringAsync();

                var authenticationResponse = JsonSerializer.Deserialize<AuthenticationResponse>(content, jsonSerializerOptions);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", authenticationResponse.IdentityToken);

                return authenticationResponse;
            }

            throw await GetError(res);
        }
    }
}
