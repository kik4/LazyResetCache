using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_SetShould
    {
        [Fact]
        public void Success()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            cache.Set("0", () => "hoge");
        }
    }
}
