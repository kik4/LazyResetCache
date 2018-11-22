using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_ExistsShould
    {
        [Fact]
        public void ReturnsFalseFirst()
        {
            var cache = new LazyResetCache<string>();
            Assert.False(cache.Exists("test"), "should be null");
        }

        [Fact]
        public void ReturnsTrueWhenHasCache()
        {
            var cache = new LazyResetCache<string>();
            cache.Set("hoge", () => "piyo");
            Assert.True(cache.Exists("hoge"), "should have something");
        }
    }
}
