namespace Sfa.Eds.Das.Web
{
    using Core.Logging;
    using System;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Http;
    using System.Web.Configuration;

    using Microsoft.ApplicationInsights.Extensibility;

    public class MvcApplication : System.Web.HttpApplication
    {
        private ILog _logger;

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            _logger = DependencyResolver.Current.GetService<ILog>();

            _logger.Info("Starting web applications...");

            SetupApplicationInsights();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _logger.Info("Web applications started...");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();

            _logger.Error(ex, "App_Error");
        }

        private void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = WebConfigurationManager.AppSettings["iKey"];

            TelemetryConfiguration.Active.ContextInitializers.Add(new ApplicationInsightsInitializer());
        }
    }
}
