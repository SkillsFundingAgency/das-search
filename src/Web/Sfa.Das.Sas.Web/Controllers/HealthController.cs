namespace Sfa.Das.Sas.Web.Controllers
{
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Health;
    using Newtonsoft.Json;

    public class HealthController : Controller
    {
        private readonly IHealthService _healthService;

        public HealthController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        // GET: Health
        public ActionResult Start()
        {
            var viewModel = _healthService.CreateModel();

            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            viewModel.WebAppVersion = version;

            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Content(JsonConvert.SerializeObject(viewModel));
            }

            return View(viewModel);
        }
    }
}