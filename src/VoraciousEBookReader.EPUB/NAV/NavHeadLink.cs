using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.NAV
{
    public partial class NavHeadLink
    {
        internal static class Attributes
        {
            public static readonly XName Href = "href";
            public static readonly XName Rel = "rel";
            public static readonly XName Type = "type";
            public static readonly XName Class = "class";
            public static readonly XName Title = "title";
            public static readonly XName Media = "media";
        }

        public string Href { get; internal set; }
        public string Rel { get; internal set; }
        public string Type { get; internal set; }
        public string Class { get; internal set; }
        public string Title { get; internal set; }
        public string Media { get; internal set; }
    }
}
