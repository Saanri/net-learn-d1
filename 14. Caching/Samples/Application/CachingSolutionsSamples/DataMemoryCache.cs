using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;
using System.Data.SqlClient;

namespace CachingSolutionsSamples
{
    internal class DataMemoryCache : ITableCache
    {
        ObjectCache cache = MemoryCache.Default;
        private readonly string _prefix = "Cache_";
        private readonly string _nameCache;

        public DataMemoryCache(string nameCache)
        {
            _nameCache = nameCache;
        }

        public IEnumerable<T> Get<T>(string forUser) where T : new()
        {
            return (IEnumerable<T>)cache.Get(_prefix + _nameCache + forUser);
        }

        public void Set<T>(string forUser, IEnumerable<T> data)
        {
            cache.Set(_prefix + _nameCache + forUser, data, new DateTimeOffset(DateTime.UtcNow.AddMinutes(0.1)));
        }

        public void Set<T>(string forUser, IEnumerable<T> data, CacheItemPolicy policy)
        {
            cache.Set(_prefix + _nameCache + forUser, data, policy);
        }

        public void Remove(string forUser)
        {
            return;
        }

    }
}
