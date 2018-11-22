using System;
using System.Runtime.Caching;

namespace LazyResetCache
{
    public class LazyResetCache<T>
    {
        static readonly string _seperator = "/";
        MemoryCache _cache = MemoryCache.Default;

        public void Set(string key, Func<T> setter)
        {
            this._cache[this.GetFullKey(key)] = setter();
        }

        public T Get(string key)
        {
            return (T)this._cache[this.GetFullKey(key)];
        }

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
