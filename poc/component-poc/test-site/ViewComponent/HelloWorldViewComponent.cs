using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace test_site
{
    public class HelloWorldViewComponent : ViewComponent
    {
        private readonly IConfiguration _config;

        public HelloWorldViewComponent(IConfiguration config)
        {
            _config = config;
        }

        public IViewComponentResult Invoke(string name)
        {
            var message = _config.GetValue<string>("message") + name;

            return View((object)message);
        }
    }
}