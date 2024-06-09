using System;
using System.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Services.Services
{
    public class DataCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public DataCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public DataTable GetCachedData(string key)
        {
            if (!_memoryCache.TryGetValue(key, out DataTable data))
            {
                data = LoadFromDB();
                _memoryCache.Set(key, data, TimeSpan.FromMinutes(30));
            }
            return data;
        }

        private DataTable LoadFromDB()
        {
            return new DataTable();
        }
    }
}

