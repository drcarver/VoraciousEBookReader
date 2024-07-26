using System.Collections.Generic;
using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.NCX
{
    public class NcxNavPoint
    {
        internal static class Attributes
        {
            public static readonly XName Id = "id";
            public static readonly XName Class = "class";
            public static readonly XName PlayOrder = "playOrder";
            public static readonly XName ContentSrc = "src";
        }

        public string Id { get; internal set; }
        public string Class { get; internal set; }
        public int? PlayOrder { get; internal set; }
        // NavLabelText and ContentSrc are flattened elements for convenience.
        // In case <navLabel> or <content/> need to carry more data, then they should have a dedicated model created.
        public string NavLabelText { get; internal set; }
        public string ContentSrc { get; internal set; }
        public IList<NcxNavPoint> NavPoints { get; internal set; } = new List<NcxNavPoint>();

        public override string ToString()
        {
            return $"Id: {Id}, ContentSource: {ContentSrc}";
        }
    }
}
