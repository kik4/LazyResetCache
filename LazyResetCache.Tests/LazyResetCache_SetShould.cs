using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_SetShould
    {
        [Fact]
        public void Success()
        {
            var cache = new LazyResetCache<string>();
            cache.Set("0", "hoge");
        }
    }
}
