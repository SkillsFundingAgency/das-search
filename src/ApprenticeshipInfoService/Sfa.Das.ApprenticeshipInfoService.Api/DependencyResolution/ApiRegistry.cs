using System.Web;
using Sfa.Das.ApprenticeshipInfoService.Api.Logging;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;
using StructureMap;

namespace Sfa.Das.ApprenticeshipInfoService.Api.DependencyResolution
{
    public sealed class ApiRegistry : Registry
    {
        public ApiRegistry()
        {
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));
        }
    }
}
