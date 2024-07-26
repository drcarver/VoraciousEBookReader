using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.NCX
{
    public class NcxPageTarget
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Class = "class";
            public static readonly XName Type = "type";
            public static readonly XName Value = "value";
            public static readonly XName ContentSrc = "src";
        }

        public string Id { get; internal set; }
        public string Value { get; internal set; }
        public string Class { get; internal set; }
        public NcxPageTargetType? Type { get; internal set; }
        public string NavLabelText { get; internal set; }
        public string ContentSrc { get; internal set; }
    }
}
