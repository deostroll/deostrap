using System.Web;
using System.Web.Optimization;

namespace Bootstrap.Mvc.Demo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/jquery-{version}.js",
                    "~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css"));            
        }
    }
}