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

        // GET: Health2
        [OutputCache(Duration = 10)]
        public ActionResult Index()
        {
            var hej = _healthService.CreateModel();

            if (Request.AcceptTypes.Contains("application/json"))
            {
                return Content(JsonConvert.SerializeObject(hej));
            }

            return this.View(hej);
        }
    }
}