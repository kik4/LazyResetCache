using System;
using Xunit;
using System.Threading.Tasks;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_GetShould
    {
        [Fact]
        public void ReturnsCachedValue()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0), () => "piyo");
            Assert.Equal("piyo", cache.Get());
        }

        [Fact]
        public async void Expires()
        {
            var i = 0;
            var cache = new LazyResetCache<int>(new TimeSpan(0, 0, 0, 0, 100), () =>
            {
                Task.Delay(100).Wait();
                return i;
            });
            Assert.Equal(0, cache.Get());

            await Task.Delay(2000);
            i++;
            Assert.Equal(0, cache.Get());
            await Task.Delay(2000);
            Assert.Equal(1, cache.Get());
        }
    }
}
