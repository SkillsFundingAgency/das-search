using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace test_site
{
    public class WelcomeViewComponent : ViewComponent
    {
        private readonly IConfiguration _config;

        public WelcomeViewComponent(IConfiguration config)
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