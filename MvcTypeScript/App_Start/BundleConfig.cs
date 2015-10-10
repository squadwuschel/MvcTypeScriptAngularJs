using System.Web;
using System.Web.Optimization;

namespace MvcTypeScript
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-ui-router.js",
                        "~/Scripts/angular-locale_de-de.js",
                        "~/Scripts/angular-tablescroll.js",
                        "~/Scripts/angular-strap.js",
                        "~/Scripts/angular-strap.tpl.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                        "~/Scripts/angular-scrollable-table.js",
                        "~/Scripts/moment-with-locales.min.js"
                        ));

            //Acthung hier ist die richtige Reihenfolge der Dateien wichtig, wegen der abhängkeiten zwischen den Modulen
            bundles.Add(new ScriptBundle("~/bundles/mainApp")
                .IncludeDirectory("~/ScriptsApp/directives", "*.js", true)
                //.IncludeDirectory("~/ScriptsApp/views/Shared", "*.js", true)
                .Include(
                        "~/Scripts/Enums.js",
                        "~/ScriptsApp/Views/routeConfig.js",
                        "~/ScriptsApp/Views/mainCtrl.js",
                        "~/ScriptsApp/Services/todoPSrv.js",
                        "~/ScriptsApp/Views/Shared/todoListenService.js",
                        "~/ScriptsApp/Views/Shared/todoModalService.js",
                        "~/ScriptsApp/Views/Todo/todooverviewctrl.js",
                        "~/ScriptsApp/Views/Todo/todoEditModalCtrl.js",
                        "~/ScriptsApp/Views/Todo/es6FeaturesCtrl.js",
                        "~/ScriptsApp/Views/app.main.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css", 
                        "~/Content/main.css",
                        "~/Content/scrollable-table.css", 
                        "~/Content/css/font-awesome.css"
                        ));

            System.Web.Optimization.BundleTable.EnableOptimizations = false;
        }
    }
}