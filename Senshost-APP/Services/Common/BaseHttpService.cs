using Senshost_APP.Models.Common;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Senshost_APP.Services.Common
{
    public abstract class BaseHttpService
    {
        readonly HttpClient httpClient;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public BaseHttpService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async virtual Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Get, endpoint);
            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken: cancellationToken);
            }

            throw await GetError(response);
        }

        public async virtual Task<T> PostAsync<T>(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint, data);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken);
            }

            throw await GetError(response);
        }

        public async virtual Task PostAsync(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw await GetError(response);
        }

        public async virtual Task<T> PutAsync<T>(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Put, endpoint);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken);
            }

            throw await GetError(response);
        }

        public async virtual Task PutAsync(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Put, endpoint);

            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw await GetError(response);
        }

        public async virtual Task<T> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Delete, endpoint);
            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions, cancellationToken);
            }

            throw await GetError(response);
        }

        public async virtual Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var request = CreateRequest(HttpMethod.Delete, endpoint);
            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw await GetError(response);
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string uri, object value = null)
        {
            var request = new HttpRequestMessage(method, uri);

            if (value != null)
                request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

            return request;
        }

        protected async Task<HttpRequestException> GetError(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var error = JsonSerializer.Deserialize<ErrorModel>(content, jsonSerializerOptions);

                error.Message = !string.IsNullOrEmpty(error.ErrorMessage) ? error.ErrorMessage : error.Message;
                return new HttpRequestException(error.Message, null, response.StatusCode);
            }
            catch
            {
                return new HttpRequestException(response.ReasonPhrase + " " + content, null, response.StatusCode);
            }
        }
    }
}