namespace Sfa.Das.ApprenticeshipInfoService.Api.DependencyResolution
{
    using System.Web;
    using Sfa.Das.ApprenticeshipInfoService.Api.Logging;
    using Sfa.Das.ApprenticeshipInfoService.Core.Logging;
    using StructureMap;

    public sealed class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));
        }
    }
}