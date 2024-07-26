using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfGuideReference
    {
        internal static class Attributes
        {
            public static readonly XName Title = "title";
            public static readonly XName Type = "type";
            public static readonly XName Href = "href";
        }

        public string Type { get; internal set; }
        public string Title { get; internal set; }
        public string Href { get; internal set; }

        public override string ToString()
        {
            return $"Type: {Type}, Href: {Href}";
        }
    }
}
