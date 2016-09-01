using System.Web;
using System.Web.Optimization;

namespace MetadataTool
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                        "~/Scripts/lib/tinymce_4.4.2/tinymce/js/tinymce/tinymce.min.js",
                        "~/Scripts/site.tinymce.js"));

            bundles.Add(new ScriptBundle("~/jsscripts").Include(
                        "~/Scripts/lib/handlebars-v4.0.5.js",
                        "~/Scripts/lib/lodash.core.js",
                        "~/Scripts/update-frameworks.js",
                        "~/Scripts/site.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css",
                      "~/Content/lib/font-awesome/css/font-awesome.min.css"));
        }
    }
}
