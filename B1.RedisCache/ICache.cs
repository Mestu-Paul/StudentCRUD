namespace B1.RedisCache
{
    public interface ICache
    {
        Task<T> GetData<T>(string key);
        Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);
        Task<object> RemoveData(string key);
    }
}
