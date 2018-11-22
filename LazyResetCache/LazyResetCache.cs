using System;
using System.Runtime.Caching;

namespace LazyResetCache
{
    public class LazyResetCache<T>
    {
        static readonly string _seperator = "/";
        MemoryCache _cache = MemoryCache.Default;

        public bool Exists(string key)
        {
            return this._cache[this.GetFullKey(key)] != null;
        }

        private string GetFullKey(string key)
        {
            return typeof(T).FullName + _seperator + key;
        }
    }
}
