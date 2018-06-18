using System.Web.Optimization;

namespace AnousithExpress.web.mvc
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
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/JqueryCustom")
               .Include("~/Styles/js/jquery-1.11.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/ScriptsCustom")
                .Include(

                "~/Styles/js/boostrap.js",
                "~/Styles/js/boostrap-datepicker.js",
                "~/Styles/js/customer.js",
                "~/Styles/js/npm.js",
                "~/Styles/js/lumino.glyphs.js",
                "~/Styles/js/html5shiv.min.js"));

            bundles.Add(new StyleBundle("~/Styles/css")
                .Include(
                "~/Styles/css/bootstrap.css",
                "~/Styles/css/datepicker.css",
                "~/Styles/css/datepicker3.css",
                "~/Styles/css/font-awesome.min.css",
                "~/Styles/css/styles.css",
                "~/Styles/css/boostrap-table.css",
                "~/Styles/css/bootstrap-theme.css",
                "~/LaoFont/style.css"));

        }
    }
}
