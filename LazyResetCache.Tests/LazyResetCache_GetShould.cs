using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_GetShould
    {
        [Fact]
        public void ReturnsNull()
        {
            var cache = new LazyResetCache<string>();
            Assert.Null(cache.Get("test"));
        }

        [Fact]
        public void ReturnsCachedValue()
        {
            var cache = new LazyResetCache<string>();
            cache.Set("hoge", "piyo");
            Assert.Equal("piyo", cache.Get("hoge"));
        }
    }
}
