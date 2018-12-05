using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_ExistsShould
    {
        [Fact]
        public void ReturnsFalseFirst()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            Assert.False(cache.Exists(), "should be null");
        }

        [Fact]
        public void ReturnsTrueWhenHasCache()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            cache.Init(() => "piyo");
            Assert.True(cache.Exists(), "should have something");
        }
    }
}
