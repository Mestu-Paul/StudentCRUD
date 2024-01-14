namespace B1.RedisCache
{
    public interface ICache
    {
        Task<T> GetData<T>(string key);
        Task<T> GetData<T>(int page, int pageSize);
        Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);
        Task<bool> SetData<T>(int page, int pageSize, T value, DateTimeOffset expirationTIme);
        Task<object> RemoveData(string key);

        Task ClearCache();
    }
}
