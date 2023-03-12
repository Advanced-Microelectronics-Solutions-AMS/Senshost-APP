using Senshost.Common.Interfaces;

namespace Senshost.Services
{
    public class SecureStorageService : IStorageService
    {
        public async Task SetAsync(string key, string value)
        {
            await SecureStorage.Default.SetAsync(key, value);
        }

        public async Task<string> GetAsync(string key)
        {
            return await SecureStorage.Default.GetAsync(key);
        }

        public bool Remove(string key)
        {
            return SecureStorage.Default.Remove(key);
        }

        public void RemoveAll()
        {
            SecureStorage.Default.RemoveAll();
        }
    }
}
