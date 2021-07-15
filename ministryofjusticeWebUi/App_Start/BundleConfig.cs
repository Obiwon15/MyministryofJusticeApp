using System.Web.Optimization;

namespace ministryofjusticeWebUi
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            */

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //        "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Static/css").Include(
                "~/Static/css/fonts/icomoon/style.css",
                "~/Static/css/bootstrap.min.css",
                "~/Static/css/bootstrap-datepicker.css",
                "~/Static/css/jquery.fancybox.min.css",
                "~/Static/css/owl.carousel.min.css",
                "~/Static/css/owl.theme.default.min.css",
                "~/Static/fonts/flaticon/font/flaticon.css",
                "~/Static/css/aos.css",
                "~/Static/css/style.css"
            ));

            bundles.Add(new ScriptBundle("~/Static/js").Include(
                "~/Static/js/jquery-3.3.1.min.js",
                "~/Static/js/popper.min.js",
                "~/Static/js/bootstrap.min.js",
                "~/Static/js/owl.carousel.min.js",
                "~/Static/js/jquery.sticky.js",
                "~/Static/js/jquery.animateNumber.min.js",
                "~/Static/js/jquery.waypoints.min.js",
                "~/Static/js/jquery.fancybox.min.js",
                "~/Static/js/jquery.easing.1.3.js",
                "~/Static/js/bootstrap-datepicker.min.js",
                "~/Static/js/aos.js",
                "~/Static/js/main.js"));

            bundles.Add(new ScriptBundle("~/Scripts/main").Include(
                "~/Static/js/main.js"));

            bundles.Add(new ScriptBundle("~/Scripts/google-map").Include(
                "~/Static/js/google-map.js"));


            bundles.Add(new StyleBundle("~/Assets/fontawesome").Include(
                "~/Assets/vendor/fontawesome-free/css/all.min.css"));

            bundles.Add(new StyleBundle("~/Assets/styles").Include(
                "~/Assets/css/sb-admin-2.min.css"));

            bundles.Add(new StyleBundle("~/Assets/main").Include(
                "~/Assets/css/styles.css"
            ));

            bundles.Add(new StyleBundle("~/Assets/vendors").Include(
                "~/Assets/vendor/datatables/dataTables.bootstrap4.min.css",
                "~/Assets/vendor/daterangepicker/daterangepicker.css"
            ));

            bundles.Add(new ScriptBundle("~/Assets/scripts").Include(
                "~/Assets/vendor/jquery/jquery.min.js",
              "~/Assets/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Assets/vendor/jquery-easing/jquery.easing.min.js",
                "~/Assets/js/sb-admin-2.min.js",
                "~/Assets/js/scripts.js"
            ));
            bundles.Add(new ScriptBundle("~/Assets/plugins").Include(
                "~/Assets/vendor/email-decode/email-decode.min.js",
                "~/Assets/vendor/chart.js/Chart.min.js",
                "~/Assets/js/demo/chart-area-demo.js",
                "~/Assets/js/demo/chart-pie-demo.js",
                "~/Assets/js/demo/datatables-demo.js",
                "~/Assets/vendor/moment/moment.min.js",
                "~/Assets/vendor/daterangepicker/daterangepicker.min.js",
                "~/Assets/js/demo/date-range-picker-demo.js"
            ));

            bundles.Add(new ScriptBundle("~/Assets/customizer").Include(
                "~/Assets/js/sb-customizer.js"
            ));
        }
    }
}