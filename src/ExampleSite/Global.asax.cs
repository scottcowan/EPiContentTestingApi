using System.Collections.Generic;
using System.Web.Mvc;

namespace ExampleSite
{
    public class Global : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }

        public static Dictionary<string, int> ContentAreaTagWidths = new Dictionary<string, int>
                    {
                        { ContentAreaTags.FullWidth, ContentAreaWidths.FullWidth },
                        { ContentAreaTags.TwoThirdsWidth, ContentAreaWidths.TwoThirdsWidth },
                        { ContentAreaTags.HalfWidth, ContentAreaWidths.HalfWidth },
                        { ContentAreaTags.OneThirdWidth, ContentAreaWidths.OneThirdWidth }
                    };

        /// <summary>
        /// Names used for UIHint attributes to map specific rendering controls to page properties
        /// </summary>
        public static class SiteUIHints
        {
            public const string Contact = "contact";
            public const string Strings = "StringList";
        }

        /// <summary>
        /// Virtual path to folder with static graphics, such as "~/Static/gfx/"
        /// </summary>
        public const string StaticGraphicsFolderPath = "~/Static/gfx/";
    }
}