namespace Sfa.Das.Sas.MetadataTool.Web.Controllers
{
    using System.Web.Mvc;

    using Sfa.Das.Sas.Core.Logging;

    public class HomeController : Controller
    {
        private readonly ILog logger;

        public HomeController(ILog logger)
        {
            this.logger = logger;
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}
