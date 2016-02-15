namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Configuration;
    using System.Web.Http;

    public class VersionController : ApiController
    {
        // GET: api/Version
        public string Get()
        {
            return ConfigurationManager.AppSettings["BuildId"];
        }
    }
}
