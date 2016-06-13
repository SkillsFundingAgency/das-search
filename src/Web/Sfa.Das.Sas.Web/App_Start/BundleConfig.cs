using System.Web.Optimization;

namespace Sfa.Das.Sas.Web
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/styles/").Include(
                      "~/Content/dist/css/screen.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/scripts/polyfills/*.js",
                "~/scripts/appsettings.js",
                "~/scripts/app/*.js",
                "~/scripts/pages/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/header").Include(
                "~/scripts/js-errors.js",
                "~/scripts/vendor/modernizr-3.3.1.min.js"));
        }
    }
}
