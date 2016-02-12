namespace Sfa.Eds.Das.Web
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using log4net;
    using System.Web.Http;

    using Sfa.Eds.Das.Infrastructure.Logging;

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger("MainLogger");

        protected void Application_Start()
        {
            var logserver = ConfigurationManager.AppSettings["ElasticServerIp"];
            Log4NetSettings.Initialise(logserver);

            Log.Info("Starting web applications...");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Log.Info("Web applications started...");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();

            Log.Error("App_Error", ex);
        }
    }
}
