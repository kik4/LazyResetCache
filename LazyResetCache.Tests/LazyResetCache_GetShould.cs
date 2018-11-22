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

#pragma warning disable 1998
        [Fact]
        public void ReturnsCachedValue()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            cache.Set("hoge", async () => "piyo");
            Assert.Equal("piyo", cache.Get("hoge"));
        }
#pragma warning restore 1998

        [Fact]
        public async void Expires()
        {
            var cache = new LazyResetCache<int>((new TimeSpan(0, 0, 0, 0, 100)));
            var i = 0;
            cache.Set("counter", async () =>
            {
                await Task.Delay(50);
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
