using System.Web.Mvc;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class ErrorController : Controller
    {
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