namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;

    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Core.Domain.Services;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Web.Extensions;
    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    public sealed class StartController : Controller
    {
        private readonly ILog _logger;

        public StartController(ILog logger)
        {
            _logger = logger;
        }

        public ActionResult Start()
        {
            return View();
        }
    }
}