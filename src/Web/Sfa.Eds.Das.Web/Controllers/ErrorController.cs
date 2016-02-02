namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Mvc;
    public class ErrorController : Controller
    {
        // GET: Error
        public ViewResult NotFound(string source)
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