using System;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.ApplicationInsights.Extensibility;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.Web
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using StackExchange.Profiling;
    using StackExchange.Profiling.Mvc;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            var logger = DependencyResolver.Current.GetService<ILog>();

            logger.Info("Starting Web Role");

            MiniProfiler.Settings.Results_Authorize = IsUserAllowedToSeeMiniProfilerUI;
            SetupApplicationInsights();
            ProfileViewEngines();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Web Role started");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            var logger = DependencyResolver.Current.GetService<ILog>();
            logger.Error(ex, "App_Error");
        }

        protected void Application_BeginRequest()
        {
            MiniProfiler.Start();
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        private void SetupApplicationInsights()
        {
            TelemetryConfiguration.Active.InstrumentationKey = WebConfigurationManager.AppSettings["iKey"];

            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ApplicationInsightsInitializer());
        }

        private void ProfileViewEngines()
        {
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }

        private bool IsUserAllowedToSeeMiniProfilerUI(HttpRequest httpRequest)
        {
            // Implement your own logic for who
            // should be able to access ~/mini-profiler-resources/results
            // var principal = httpRequest.RequestContext.HttpContext.User;
            // return principal.IsInRole("Developer");
            return httpRequest.Headers["profile"] != null;
        }
    }
}
