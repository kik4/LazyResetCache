using System;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace LazyResetCache
{
    public class LazyResetCache<T>
    {
        static readonly string _seperator = "/";
        static MemoryCache _cache = MemoryCache.Default;
        TimeSpan _span;
        object _lock = new object();

        public LazyResetCache(TimeSpan span)
        {
            this._span = span;
        }

        public void Init(Func<T> setter)
        {
            if (Monitor.TryEnter(this._lock))
            {
                try
                {
                    var item = new CacheItem<T> { value = setter(), setter = setter, expiredTime = this.CulcExpiredTime() };
                    this._Set(item);
                }
                finally
                {
                    Monitor.Exit(this._lock);
                }
            }
        }

        public T Get()
        {
            if (!this.Exists())
            {
                return default(T);
            }

            var now = DateTime.Now;
            var expiredTime = (DateTime)this._Get().expiredTime;
            if (DateTime.Compare(now, expiredTime) >= 0)
            {
                if (Monitor.TryEnter(this._lock))
                {
                    try
                    {
                        this._Get().expiredTime = this.CulcExpiredTime();
                        Func<Task> resetter = async () =>
                        {
                            var setter = this._Get().setter;
                            this._Get().value = await Task.Run(() => setter());
                        };
                        resetter();
                    }
                    finally
                    {
                        Monitor.Exit(this._lock);
                    }
                }
            }
            return this._Get().value;
        }

        public bool Exists()
        {
            return this._Get() != null;
        }

        private string GetFullKey()
        {
            return this.GetHashCode() + _seperator + typeof(T).FullName + _seperator + this.GetHashCode();
        }

        private DateTime CulcExpiredTime()
        {
            return DateTime.Now + this._span;
        }

        private void _Set(CacheItem<T> item)
        {
            _cache[this.GetFullKey()] = item;
        }

        private CacheItem<T> _Get()
        {
            return (CacheItem<T>)_cache[this.GetFullKey()];
        }
    }

    class CacheItem<T>
    {
        public Func<T> setter;
        public T value;
        public DateTime expiredTime;
    }
}
