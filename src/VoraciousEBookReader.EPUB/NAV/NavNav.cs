using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.NAV
{
    public class NavNav
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Class = "class";
            public static readonly XName Type = Constants.OpsNamespace + "type";
            public static readonly XName Hidden = Constants.OpsNamespace + "hidden";

            internal static class TypeValues
            {
                public const string Toc = "toc";
                public const string Landmarks = "landmarks";
                public const string PageList = "page-list";
            }
        }

        /// <summary>
        /// Instantiated only when the EPUB was read.
        /// </summary>
        internal XElement Dom { get; set; }

        public string Type { get; internal set; }
        public string Id { get; internal set; }
        public string Class { get; internal set; }
        public string Hidden { get; internal set; }
    }
}
