using System.Text.Json;
using StackExchange.Redis;

namespace B1.RedisCache
{
    public class Cache : ICache
    {
        private readonly IDatabase _cacheDb;

        public Cache()
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect("localhost:6379");
                _cacheDb = redis.GetDatabase();
            }
            catch (Exception e)
            {
                 // throw new RedisConnectionException(ConnectionFailureType.UnableToConnect, "Can not connect with cache");
            }
            
        }
        public async Task<T> GetData<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
                return await JsonSerializer.DeserializeAsync<T>(new MemoryStream(value));
            return default;
        }

        public async Task<T> GetData<T>(int page, int pageSize)
        {
            string key = page.ToString() + "-" + pageSize.ToString();
            return await this.GetData<T>(key);
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

        private async Task AddPageToQueue(string key)
        {
            await _cacheDb.ListRightPushAsync("page", key);
            long queueLength = _cacheDb.ListLength("page");
            if (queueLength > 5)
            {
               await _cacheDb.ListLeftPopAsync("page");
            }
        }

        public async Task<bool> SetData<T>(int page, int pageSize, T value, DateTimeOffset expirationTIme)
        {
            string key = page.ToString() + "-" + pageSize.ToString();
            this.AddPageToQueue(key);
            var isSet = await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value));
            return isSet;
        }

        public async Task ClearCache()
        {
            await _cacheDb.ExecuteAsync("FLUSHDB");
        }

    }
}
