namespace Sfa.Das.ApprenticeshipInfoService.Api.Attributes
{
    using System.Web.Http.Filters;
    using System.Web.Mvc;

    using Sfa.Das.ApprenticeshipInfoService.Core.Logging;

    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Error(context.Exception, "App_Error");
        }
    }
}