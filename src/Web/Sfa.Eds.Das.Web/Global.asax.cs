namespace Sfa.Eds.Das.Web
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using log4net;
    using System.Web.Http;

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger("HomeController");

        protected void Application_Start()
        {
            Log.Info("Starting web applications...");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Log4NetSettings.Initialise();

            Log.Info("Web applications started...");
        }
    }
}
