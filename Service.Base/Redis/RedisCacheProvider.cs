using System;
using ServiceStack.Redis;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        public readonly IRedisClientsManager _redisClientManager;
        public RedisCacheProvider(IRedisClientsManager redisClientManager)
        {
            _redisClientManager = redisClientManager;
        }

        public bool IsInCache(string key)
        {
            bool isInCache = false;
            
            using(var client = _redisClientManager.GetClient())
            {
                isInCache = client.ContainsKey(key);
            }

            return isInCache;
        }

        public bool Remove(string key)
        {
            bool removed = false;

            using(var client = _redisClientManager.GetClient())
            {
                removed = client.Remove(key);
            }

            return removed;
        }

        public void Set<T>(string key, T value)
        {
            this.Set(key, value, TimeSpan.Zero);
        }

        public void Set<T>(string key, T value, TimeSpan timeout)
        {
            using (var client = _redisClientManager.GetClient())
            {
                client.As<T>().SetValue(key, value, timeout);
            }
        }

        public T Get<T>(string key)
        {
            T result = default(T);

            using(var client = _redisClientManager.GetClient())
            {
                var wrapper = client.As<T>();

                result = wrapper.GetValue(key);
            }

            return result;
        }
    }
}
