using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.NAV
{
    public class NavMeta
    {
        public static class Attributes
        {
            public static readonly XName Name = "name";
            public static readonly XName Content = "content";
            public static readonly XName Charset = "charset";
        }

        public string Name { get; internal set; }
        public string Content { get; internal set; }
        public string Charset { get; internal set; }
    }
}
