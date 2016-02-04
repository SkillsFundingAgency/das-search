namespace Sfa.Eds.Das.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using log4net;

    public class ErrorController : Controller
    {
        private readonly ILog logger;

        public ErrorController(ILog logger)
        {
            this.logger = logger;
        }

        // GET: Error
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View("_Error404");
        }

        public ViewResult Error()
        {
            Response.StatusCode = 500;
            return View("_Error500");
        }
    }
}