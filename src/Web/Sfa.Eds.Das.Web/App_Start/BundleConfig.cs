using System.Web;
using System.Web.Optimization;

namespace Sfa.Eds.Das.Web
{
    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/styles").Include(
                      "~/Content/css/fonts.css",
                      "~/Content/css/screen.min.css"));
        }
    }
}
