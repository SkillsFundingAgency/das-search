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
                "~/scripts/accessible-autocomplete.min.js",
                "~/scripts/polyfills/*.js",
                "~/scripts/pages/*.js",
                "~/scripts/app/*.js"));

            var header = new ScriptBundle("~/bundles/header")
                .Include("~/scripts/js-errors.js")
                .Include("~/scripts/vendor/modernizr-3.3.1.min.js");
            header.Orderer = new AsIsBundleOrderer();
            bundles.Add(header);
        }
    }
}
