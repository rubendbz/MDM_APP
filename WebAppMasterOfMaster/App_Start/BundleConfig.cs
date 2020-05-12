
using System.Web.Optimization;

namespace WebAppMasterOfMaster
{
    public class BundleConfig
    {
         public static void RegisterBundles(BundleCollection bundles)
        {


               bundles.Add(new StyleBundle("~/bundle/css").Include(
                "~/Contenido/vendor/bootstrap/css/bootstrap.min.css",
                "~/Contenido/vendor/fontawesome-free/css/all.min.css",
                "~/Contenido/vendor/datatables/dataTables.bootstrap4.css",
                "~/Contenido/css/sb-admin.css",
                "~/Contenido/css/StyleMenu.css"));

            bundles.Add(new ScriptBundle("~/bundle/js").Include(
                "~/Contenido/vendor/jquery/jquery.min.js",
                "~/Contenido/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Contenido/vendor/jquery-easing/jquery.easing.min.js",
                "~/Contenido/vendor/chart.js/Chart.min.js",
                "~/Contenido/vendor/datatables/jquery.dataTables.js",
                "~/Contenido/vendor/datatables/dataTables.bootstrap4.js",
                "~/Contenido/js/sb-admin.min.js",
                //"~/Contenido/js/demo/datatables-demo.js",
                "~/Contenido/js/demo/chart-area-demo.js"));

        }
    }
}