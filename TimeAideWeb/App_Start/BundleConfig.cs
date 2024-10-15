using System.Web;
using System.Web.Optimization;

namespace TimeAide.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;
            bundles.IgnoreList.Clear(); // required
                                        // Code removed for clarity.
            BundleTable.EnableOptimizations = false;
            //JQuery js file
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Content/Themes/assets/js/jquery-3.2.1.min.js"));
            //Jquery-ui
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include("~/Content/Themes/assets/js/jquery-ui.min.js"));
            //JQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            //Bootstrap js file
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Content/Themes/assets/js/popper.min.js",
                "~/Content/Themes/assets/js/bootstrap.min.js",
                "~/Content/Themes/assets/js/respond.js"));

            //SignalR js file
            bundles.Add(new ScriptBundle("~/bundles/3rdParty").Include(
                "~/Scripts/jquery.signalR-2.2.2.min.js",
                "~/Scripts/toastr.js"
                ));



            //Themes js file
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-theme").Include(
                      "~/Content/Themes/assets/js/jquery.slimscroll.min.js",
                      "~/Content/Themes/assets/js/select2.min.js",
                      //"~/Content/Themes/assets/js/jquery.dataTables.min.js",
                      "~/Content/Themes/assets/js/datatables.min.js",
                      //"~/Content/Themes/assets/js/dataTables.bootstrap4.min.js",
                      "~/Content/Themes/assets/js/buttons.dataTables.min.js",
                      "~/Content/Themes/assets/js/jszip.min.js",
                      "~/Content/Themes/assets/js/pdfmake.min.js",
                      "~/Content/Themes/assets/js/vfs_fonts.js",
                     "~/Content/Themes/assets/js/buttons.html5.min.js",
                      "~/Content/Themes/assets/js/buttons.print.min.js",
                      "~/Content/Themes/assets/js/moment.min.js",
                      "~/Content/Themes/assets/js/moment-with-locales.min",
                       "~/Content/Themes/assets/js/bootstrap-datetimepicker.min.js",
                       "~/Content/Themes/assets/js/jquery.maskedinput.min.js",
                       "~/Scripts/daterangepicker.js",
                        "~/Scripts/jquery.timepicker.js",
                        "~/Content/Themes/assets/js/app.js"
                      ));

            //Theme css
            bundles.Add(new StyleBundle("~/content/css").Include(
                      "~/Content/Themes/assets/css/bootstrap.min.css",
                      "~/Content/Themes/assets/css/font-awesome.min.css",
                      "~/Content/Themes/assets/css/line-awesome.min.css",
                     //"~/Content/Themes/assets/css/dataTables.bootstrap4.min.css",
                     "~/Content/Themes/assets/css/jquery.dataTables.min.css",
                     "~/Content/Themes/assets/css/buttons.dataTables.min.css",
                      "~/Content/Themes/assets/css/select2.min.css",
                      //  "~/Content/jquery-ui.css",
                      "~/Content/Themes/assets/css/bootstrap-datetimepicker.min.css",
                       "~/Content/Themes/assets/css/style.css",
                       "~/Content/daterangepicker.css",
                        "~/Content/jquery.timepicker.css",
                        "~/Content/toastr.css",
                         "~/Content/loader.css"
                      ));


            /* old before new theme
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js")
                        );

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/site.css"));
         */
        }
    }
}
