using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Web
{

    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog _logger;

        public MvcApplication()
        {
            _logger = DependencyResolver.Current.GetService<ILog>();
        }

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Info("Starting Web Role");

            SetupApplicationInsights();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Web Role started");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError().GetBaseException();
            var logger = DependencyResolver.Current.GetService<ILog>();

            if (ex is HttpException)
            {
                var statusCode = ((HttpException)ex).GetHttpCode();
                if (statusCode == 404 || ex.Message.Contains("Request.Path"))
                {
                    logger.Info(ex.Message);
                }
                else
                {
                    logger.Error(ex, "App_Error - HttpException");
                }
            }
            else
            {
                logger.Error(ex, "App_Error");
            }
        }

        private bool UrlContains(string text)
        {
            var url = HttpContext.Current.Request.RequestContext.HttpContext.Request.Url;
            if (url == null)
            {
                return false;
            }

            return url.OriginalString.Contains(text);
        }

        protected void Application_BeginRequest()
        {
            _logger = DependencyResolver.Current.GetService<ILog>();

            HttpContext context = base.Context;

            _logger.Info($"{context.Request.HttpMethod} {context.Request.Url.PathAndQuery}");
        }

        protected void Application_EndRequest()
        {
        }

        private void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = CloudConfigurationManager.GetSetting("InstrumentationKey");

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}
