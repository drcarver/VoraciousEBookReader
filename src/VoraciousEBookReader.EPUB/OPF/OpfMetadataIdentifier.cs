using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfMetadataIdentifier
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Scheme = "scheme";
        }

        public string Id { get; internal set; }
        public string Scheme { get; internal set; }
        public string Text { get; internal set; }
    }
}
