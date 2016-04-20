namespace Sfa.Eds.Das.Web.Controllers
{
    using System.Reflection;
    using System.Web.Http;

    using Sfa.Eds.Das.Core.Configuration;
    using Sfa.Eds.Das.Web.ViewModels;

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
            return new VersionInformation
                       {
                           BuildId = _configurationSetttings.BuildId,
                           Version = version.Remove(version.Length - 2),
                           AssemblyVersion = version
                       };
        }
    }
}