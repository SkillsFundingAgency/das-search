namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Web.Http;
    using System.Configuration;

    public class VersionController : ApiController
    {
        // GET: api/Version
        public string Get()
        {
            return ConfigurationManager.AppSettings["BuildId"];
        }
    }
}
