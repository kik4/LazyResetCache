using System;
using Xunit;
using System.Threading.Tasks;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_GetShould
    {
        [Fact]
        public void ReturnsNull()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            Assert.Null(cache.Get("test"));
        }

        [Fact]
        public void ReturnsCachedValue()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            cache.Set("hoge", () => "piyo");
            Assert.Equal("piyo", cache.Get("hoge"));
        }

        [Fact]
        public async void Expires()
        {
            var cache = new LazyResetCache<int>((new TimeSpan(0, 0, 0, 0, 100)));
            var i = 0;
            cache.Set("counter", () =>
            {
                Task.Delay(100).Wait();
                return i;
            });
            Assert.Equal(0, cache.Get("counter"));

            await Task.Delay(200);
            i++;
            Assert.Equal(0, cache.Get("counter"));
            await Task.Delay(200);
            Assert.Equal(1, cache.Get("counter"));
        }
    }
}
