using System.Web;
using System.Web.Optimization;

namespace GoalWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

//            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
//                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/jquery-1.10.2.js",
                      "~/Scripts/jquery-ui-1.10.2.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/jquery.magnific-popup.js",
                      "~/Scripts/tracker.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Jquery-ui-1.10.3.css",
                      "~/Content/jchartfx.css",
                      "~/Content/bootstrap/bootstrap.css",
                      "~/Content/bootstrap-responsive.css",
                      "~/Content/bootswatch.css",
                      "~/Content/fullcalendar.css",
                      "~/Content/datepicker.css",
                      "~/Content/magnific-popup.css",
                      "~/Content/pick-a-color-1.1.7.min.css",
                      "~/Content/sticky-footer.css",
                      "~/Content/site.css"));
        }
    }
}
