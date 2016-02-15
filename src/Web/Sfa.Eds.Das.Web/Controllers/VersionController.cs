namespace Sfa.Eds.Das.Web.Controllers
{
    using Core.Configuration;
    using System.Web.Http;

    public sealed class VersionController : ApiController
    {
        private readonly IConfigurationSettings _configurationSetttings;

        public VersionController(IConfigurationSettings settings)
        {
            _configurationSetttings = settings;
        }

        // GET: api/Version
        public string Get()
        {
            return _configurationSetttings.BuildId;
        }
    }
}
