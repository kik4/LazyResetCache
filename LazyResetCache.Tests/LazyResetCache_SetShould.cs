using System;
using Xunit;

namespace LazyResetCache.Tests
{
    public class LazyResetCache_SetShould
    {
#pragma warning disable 1998
        [Fact]
        public void Success()
        {
            var cache = new LazyResetCache<string>(new TimeSpan(1, 0, 0));
            cache.Set("0", async () => "hoge");
        }
#pragma warning restore 1998
    }
}
