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
                cache.Set(key, () =>
                {
                    Console.WriteLine("Start Set(): " + DateTime.Now.ToString());
                    Task.Delay(1000).Wait();
                    var time = DateTime.Now.ToString();
                    Console.WriteLine("Complete Set()." + time);
                    return time;
                });
            }
            return $"{DateTime.Now.ToString()} requested\n{cache.Get(key)} {(cached ? "from" : "new")} lazy cache";
        }
    }
}
