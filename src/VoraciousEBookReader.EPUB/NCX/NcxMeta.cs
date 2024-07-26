using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.NCX
{
    public class NcxMeta
    {
        internal static class Attributes
        {
            public static readonly XName Name = "name";
            public static readonly XName Content = "content";
            public static readonly XName Scheme = "scheme";
        }

        public string Name { get; internal set; }
        public string Content { get; internal set; }
        public string Scheme { get; internal set; }
    }
}
