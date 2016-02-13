namespace Sfa.Eds.Das.Web.Controllers
{
    using Core.Configuration;
    using System.Configuration;
    using System.Web.Http;

    public sealed class VersionController : ApiController
    {
        private IConfigurationSettings _configurationSettings;

        public VersionController(IConfigurationSettings settings)
        {
            _configurationSettings = settings;
        }

        // GET: api/Version
        public string Get()
        {
            return _configurationSettings.BuildId;
        }
    }
}
