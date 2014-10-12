using System.Web;
using System.Web.Optimization;

namespace LearningRace
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/mainscripts").Include(
                        "~/Scripts/Core/Social.js",
                        "~/Scripts/Core/Utils.js",
                        "~/Scripts/Core/login.js",
                        "~/Scripts/Core/app.js",
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/gamelibs").Include(
                        "~/Scripts/kinetic-v5.0.1.js",
                        "~/Scripts/knockout-2.3.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/game").Include(
                        "~/Scripts/Game/CarSelectorModel.js",
                        "~/Scripts/Game/Core.js",
                        "~/Scripts/Game/GameModel.js",
                        "~/Scripts/Game/QuestionModel.js",
                        "~/Scripts/Game/RaceController.js"));

            bundles.Add(new ScriptBundle("~/bundles/halloffame").Include(
                        "~/Scripts/Controllers/hallOfFameController.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/stylesheets/screen.css", "~/Content/stylesheets/tinyeditor.css"));
        }
    }
}