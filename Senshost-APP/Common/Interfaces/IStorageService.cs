namespace Senshost_APP.Common.Interfaces
{
    public interface IStorageService
    {
        Task SetAsync(string key, string value);
        Task<string> GetAsync(string key);
        bool Remove(string key);
        void RemoveAll();
    }
}
