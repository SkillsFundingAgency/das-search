using System.IO;
using Sfa.Das.ApprenticeshipInfoService.Health.Models;

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

        public ActionResult Image()
        {
            var viewModel = _healthService.CreateModel();

            var dir = Server.MapPath("/content");
            var path = Path.Combine(dir, viewModel.Status + "_16.png");
            return base.File(path, "image/png");
        }
    }
}