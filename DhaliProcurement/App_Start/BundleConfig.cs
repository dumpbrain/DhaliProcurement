using System.Web;
using System.Web.Optimization;

namespace DhaliProcurement
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                //"~/Scripts/jquery-{version}.js",
                //"~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/imgdemo.js",
                        "~/Scripts/jquery.magnific-popup.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/limitless").Include(
                //"~/Scripts/loaders/pace.min.js",
                "~/Scripts/loaders/blockui.min.js",
                "~/Scripts/visualization/d3/d3.min.js",
                "~/Scripts/visualization/d3/d3_tooltip.js",
                "~/Scripts/styling/switchery.min.js",
                  "~/Scripts/selects/select2.min.js",
                "~/Scripts/styling/uniform.min.js",
                "~/Scripts/selects/bootstrap_multiselect.js",
                //"~/Scripts/ui/moment/moment.min.js",
                "~/Scripts/pickers/daterangepicker.js",
                "~/Scripts/ui/nicescroll.min.js",
                "~/Scripts/app.min.js",
                "~/Scripts/pages/layout_fixed_custom.js",
                "~/html5lightbox/html5lightbox.js",
                "~/Scripts/bootstrap-editable.min.js",
                  "~/Scripts/pages/form_layouts.js",
                  "~/Scripts/notification/pnotify.min.js",
                  "~/Scripts/buttons/spin.min.js",
                  "~/Scripts/buttons/ladda.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/base/jquery-ui.min.css",
                // "~/Content/icomoon/styles.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-datepicker.css",
                      "~/Content/core.css",
                      "~/Content/components.min.css",
                      "~/Content/bootstrap-editable.css",
                      "~/Content/colors.min.css"));

            //Added by Nabid on 26-9-17

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include("~/Scripts/typeahead.bundle*"));
            bundles.Add(new ScriptBundle("~/bundles/typeahead-bloodhound").Include("~/Scripts/bloodhound*"));
            bundles.Add(new ScriptBundle("~/bundles/typeahead-jquery").Include("~/Scripts/typeahead.jquery*"));

        }
    }
}
