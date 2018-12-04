using System;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace LazyResetCache
{
    public class LazyResetCache<T>
    {
        static readonly string _seperator = "/";
        MemoryCache _cache = MemoryCache.Default;
        TimeSpan _span;
        object _lock = new object();

        public LazyResetCache(TimeSpan span)
        {
            this._span = span;
        }

        public void Init(string key, Func<T> setter)
        {
            if (Monitor.TryEnter(this._lock))
            {
                try
                {
                    var item = new CacheItem<T> { value = setter(), setter = setter, expiredTime = this.CulcExpiredTime() };
                    this._Set(key, item);
                }
                finally
                {
                    Monitor.Exit(this._lock);
                }
            }
        }

        public T Get(string key)
        {
            if (!this.Exists(key))
            {
                return default(T);
            }

            var now = DateTime.Now;
            var expiredTime = (DateTime)this._Get(key).expiredTime;
            if (DateTime.Compare(now, expiredTime) >= 0)
            {
                if (Monitor.TryEnter(this._lock))
                {
                    try
                    {
                        this._Get(key).expiredTime = this.CulcExpiredTime();
                        Func<Task> resetter = async () =>
                        {
                            var setter = this._Get(key).setter;
                            this._Get(key).value = await Task.Run(() => setter());
                        };
                        resetter();
                    }
                    finally
                    {
                        Monitor.Exit(this._lock);
                    }
                }
            }
            return this._Get(key).value;
        }

        public bool Exists(string key)
        {
            return this._Get(key) != null;
        }

        private string GetFullKey(string key)
        {
            return this.GetHashCode() + _seperator + typeof(T).FullName + _seperator + key;
        }

        private DateTime CulcExpiredTime()
        {
            return DateTime.Now + this._span;
        }

        private void _Set(string key, CacheItem<T> item)
        {
            this._cache[this.GetFullKey(key)] = item;
        }

        private CacheItem<T> _Get(string key)
        {
            return (CacheItem<T>)this._cache[this.GetFullKey(key)];
        }
    }

    class CacheItem<T>
    {
        public Func<T> setter;
        public T value;
        public DateTime expiredTime;
    }
}
