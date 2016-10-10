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

        private readonly IHealthSettings _healthSettings;

        public HealthController(IHealthService healthService, IHealthSettings healthSettings)
        {
            _healthService = healthService;
            _healthSettings = healthSettings;
        }

        // GET: Health
        public ActionResult Start()
        {
            var viewModel = _healthService.CreateModel();

            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            viewModel.BuildId = _healthSettings.BuildId;
            viewModel.Version = version;
            viewModel.AssemblyVersion = version;

            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Content(JsonConvert.SerializeObject(viewModel));
            }

            return View(viewModel);
        }
    }
}