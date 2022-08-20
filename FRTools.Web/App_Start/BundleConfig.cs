using System.Web.Optimization;

namespace FRTools
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.ba-throttle-debounce.min.js",
                "~/Scripts/jquery.scrollTo.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/chartjs").Include(
                "~/Scripts/Chart.bundle.min.js",
                "~/Scripts/chartjs-plugin-colorschemes.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/styles/jqueryui").Include(
                "~/Content/jquery-ui.min.css",
                "~/Content/jquery-ui.structure.min.css",
                "~/Content/jquery-ui.theme.min.css"));
        }
    }
}
