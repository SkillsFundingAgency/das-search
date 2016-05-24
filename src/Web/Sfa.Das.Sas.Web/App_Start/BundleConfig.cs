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
                "~/scripts/appsettings.js",
                "~/scripts/app/*.js",
                "~/scripts/standard-detail.js",
                "~/scripts/provider-detail.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/header").Include(
                "~/scripts/app/js-errors.js",
                "~/scripts/vendor/modernizr-*.min.js"));
        }
    }
}
