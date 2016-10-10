namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Linq;
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

        public ActionResult Index()
        {
            var viewModel = _healthService.CreateModel();

            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Content(JsonConvert.SerializeObject(viewModel));
            }

            return View(viewModel);
        }
    }
}