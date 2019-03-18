using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test_site.Models;

namespace test_site.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenerateSearchResults _generator;

        public HomeController(IGenerateSearchResults generator)
        {
            _generator = generator;
        }
        
        public IActionResult Index()
        {
            var results = _generator.Generate();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
