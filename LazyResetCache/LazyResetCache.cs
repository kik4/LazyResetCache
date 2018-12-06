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

        public LazyResetCache(TimeSpan span, Func<T> setter)
        {
            var policy = CacheItemPolicy(span, setter);
            var value = setter();
            _cache.Set(GetFullKey(), value, policy);
        }

        private CacheItemPolicy CacheItemPolicy(TimeSpan span, Func<T> setter)
        {
            Console.WriteLine(DateTimeOffset.Now.Add(span));
            return new CacheItemPolicy
            {
                // AbsoluteExpiration = DateTimeOffset.Now.Add(span),
                SlidingExpiration = span,
                UpdateCallback = (args) => UpdateCallback(args, span, setter),
                // RemovedCallback = (args) => Console.WriteLine("Removed: " + DateTime.Now.ToString()),
            };
        }

        private void UpdateCallback(CacheEntryUpdateArguments args, TimeSpan span, Func<T> setter)
        {
            Console.WriteLine("First Step: " + DateTime.Now.ToString());

            args.UpdatedCacheItem = args.Source.GetCacheItem(GetFullKey());
            args.UpdatedCacheItemPolicy = new CacheItemPolicy();

            Func<Task> resetter = async () =>
            {
                Console.WriteLine("Second Step");
                var value = await Task.Run(() => setter());
                var policy = CacheItemPolicy(span, setter);
                _cache.Set(new CacheItem(GetFullKey(), value), policy);
                Console.WriteLine("Third Step");
            };
            resetter();
        }

        public T Get()
        {
            var value = _cache.Get(GetFullKey());
            return (T)value;
        }

        private string GetFullKey()
        {
            return this.GetHashCode() + _seperator + typeof(T).FullName + _seperator + this.GetHashCode();
        }
    }
}
