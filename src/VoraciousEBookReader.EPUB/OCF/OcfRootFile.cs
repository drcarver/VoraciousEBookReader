using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.OCF
{
    public class OcfRootFile
    {
        internal static class Attributes
        {
            public static readonly XName FullPath = "full-path";
            public static readonly XName MediaType = "media-type";
        }

        public string FullPath { get; internal set; }
        public string MediaType { get; internal set; }
    }
}
