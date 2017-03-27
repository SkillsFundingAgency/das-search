using System.Web;
using MediatR;

using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.Logging;
using Sfa.Das.Sas.Web.Services;
using SFA.DAS.NLog.Logger;
using StructureMap;

namespace Sfa.Das.Sas.Web.DependencyResolution
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));

            For<IMappingService>().Use<MappingService>();
            For<ICookieService>().Use<CookieService>();

            For<IHttpCookieFactory>().Use<HttpCookieFactory>();

            For<IValidation>().Use<Validation>();
        }
    }
}