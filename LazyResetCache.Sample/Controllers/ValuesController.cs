using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LazyResetCache;

namespace LazyResetCache.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        static LazyResetCache<string> cache = new LazyResetCache<string>(new TimeSpan(0, 0, 3));

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            var key = "hoge";
            var cached = cache.Exists(key);

            if (!cached)
            {
                cache.Set(key, () => DateTime.Now.ToString());
            }
            return (cached ? "from " : "new ") + "lazy cache: " + cache.Get(key);
        }
    }
}
