using System.Web.Optimization;

namespace Basbakanlik.Strateji.Cayci
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scriptLibs").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/metro.min.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/select2.full.min.js",
                        "~/Scripts/knockout-3.4.2.js",
                        "~/Scripts/timeago.js",
                        "~/Scripts/jquery.signalR-2.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/custom").Include("~/Scripts/site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/metro.min.css",
                      "~/Content/metro-icons.min.css",
                      "~/Content/select2.min.css",
                      "~/Content/metro-colors.min.css",
                      "~/Content/site.css"));
            BundleTable.EnableOptimizations = false;
        }
    }
}
