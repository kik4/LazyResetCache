using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_ExistsShould
    {
        private readonly LazyResetCache<string> _cache;

        public LazyResetCache_ExistsShould()
        {
            _cache = new LazyResetCache<string>();
        }

        [Fact]
        public void ReturnsFalseFirst()
        {
            Assert.False(_cache.Exists("test"), "should be null");
        }
    }
}
