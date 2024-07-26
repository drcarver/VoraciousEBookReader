using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfMetadataCreator
    {
        internal static class Attributes
        {
            public static readonly XName Role = Constants.OpfNamespace + "role";
            public static readonly XName FileAs = Constants.OpfNamespace + "file-as";
            public static readonly XName AlternateScript = Constants.OpfNamespace + "alternate-script";
        }

        public string Text { get; internal set; }
        public string Role { get; internal set; }
        public string FileAs { get; internal set; }
        public string AlternateScript { get; internal set; }
    }
}
