using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace LazyResetCache
{
    public class LazyResetCache<T>
    {
        static readonly string _seperator = "/";
        MemoryCache _cache = MemoryCache.Default;
        TimeSpan _span;

        public LazyResetCache(TimeSpan span)
        {
            this._span = span;
        }

        public void Set(string key, Func<Task<T>> setter)
        {
            this._cache[this.GetFullKey(key)] = setter().Result;
            this._cache[this.GetSetterKey(key)] = setter;
            this._cache[this.GetExpiredTimeKey(key)] = this.CulcExpiredTime();
        }

        public T Get(string key)
        {
            if (!this.Exists(key))
            {
                return default(T);
            }

            var now = DateTime.Now;
            var expiredTime = (DateTime)this._cache[this.GetExpiredTimeKey(key)];
            if (DateTime.Compare(now, expiredTime) >= 0)
            {
                Func<Task> f = async () =>
                {
                    var setter = (Func<Task<T>>)this._cache[this.GetSetterKey(key)];
                    this._cache[this.GetFullKey(key)] = await setter();
                    this._cache[this.GetExpiredTimeKey(key)] = this.CulcExpiredTime();
                };
                f();
            }
            return (T)this._cache[this.GetFullKey(key)];
        }

        public bool Exists(string key)
        {
            return this._cache[this.GetFullKey(key)] != null;
        }

        private string GetFullKey(string key)
        {
            return this.GetHashCode() + _seperator + typeof(T).FullName + _seperator + key;
        }

        private string GetExpiredTimeKey(string key)
        {
            return this.GetFullKey(key) + _seperator + "ExpiredTime";
        }

        private string GetSetterKey(string key)
        {
            return this.GetFullKey(key) + _seperator + "Setter";
        }

        private DateTime CulcExpiredTime()
        {
            return DateTime.Now + this._span;
        }
    }
}
