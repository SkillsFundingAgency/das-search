using System.Configuration;
using System.Reflection;
using System.Web.Http;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public sealed class VersionController : ApiController
    {
        private readonly IConfigurationSettings _configurationSetttings;

        public VersionController(IConfigurationSettings settings)
        {
            _configurationSetttings = settings;
        }

        // GET: api/Version
        public VersionInformation Get()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var buildVersion = ConfigurationManager.AppSettings["BuildVersion"];
            return new VersionInformation
            {
                BuildId = _configurationSetttings.BuildId,
                Version = buildVersion,
                AssemblyVersion = version
            };
        }
    }
}