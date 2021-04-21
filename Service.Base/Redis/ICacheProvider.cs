using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Redis
{
    public interface ICacheProvider
    {
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, TimeSpan timeout);
        T Get<T>(string key);
        bool Remove(string key);
        bool IsInCache(string key);
    }
}
