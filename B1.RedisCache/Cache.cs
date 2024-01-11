using System.Text.Json;
using StackExchange.Redis;

namespace B1.RedisCache
{
    public class Cache : ICache
    {
        private readonly IDatabase _cacheDb;

        public Cache()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }
        public async Task<T> GetData<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
                return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(value));
            return default;
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }

        public async Task<object> RemoveData(string key)
        {
            var _exist = await _cacheDb.KeyExistsAsync(key);
            if (_exist)
                return await _cacheDb.KeyDeleteAsync(key);
            return false;
        }


    }
}
