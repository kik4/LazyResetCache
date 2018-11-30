using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_InitShould
    {
        [Fact]
        public void Success()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            cache.Init("0", () => "hoge");
        }
    }
}
