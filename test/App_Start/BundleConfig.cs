using System.Web;
using System.Web.Optimization;

namespace ClothesStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/js/minicart.min.js",
                        "~/js/easy-responsive-tabs.js",
                        "~/js/jquery.waypoints.min.js",
                        "~/js/jquery.countup.js",
                        "~/js/move-top.js",
                        "~/js/imagezoom.js",
                        "~/js/jquery.flexslider.js",
                        "~/js/jquery.easing.min.js",
                        "~/js/jquery-2.1.4.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Styles/bootstrap.css",
                      "~/Styles/style.css",
                      "~/Styles/flexslider.css",
                      "~/Styles/css/font-awesome.css",
                      "~/Styles/easy-responsive-tabs.css"));
            bundles.Add(new StyleBundle("~/Content/admin-css").Include(
                "~/Styles/css/bootstrap.min.css",
                "~/Styles/css/font-awesome.min.css",
                "~/Styles/css/styles.css"));
        }
    }
}
