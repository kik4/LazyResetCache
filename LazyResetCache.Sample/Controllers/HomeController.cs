using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LazyResetCache.Sample.Models;
using LazyResetCache;

namespace LazyResetCache.Sample.Controllers
{
    public class HomeController : Controller
    {
        static LazyResetCache<string> cache = new LazyResetCache<string>(new TimeSpan(0, 0, 3));

        public IActionResult Index()
        {
            var cached = cache.Exists();

            if (!cached)
            {
                cache.Init(() =>
                {
                    Console.WriteLine("Start Set(): " + DateTime.Now.ToString());
                    Task.Delay(1000).Wait();
                    var time = DateTime.Now.ToString();
                    Console.WriteLine("Complete Set()." + time);
                    return time;
                });
            }

            ViewData["Requested"] = DateTime.Now.ToString();
            ViewData["Cached"] = cache.Get();
            ViewData["IsCached"] = cached;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
