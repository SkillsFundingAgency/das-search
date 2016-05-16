using System.Web.Mvc;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class ErrorController : Controller
    {
        // GET: Error
        public ViewResult BadRequest()
        {
            Response.StatusCode = 400;

            return View("_Error400");
        }

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