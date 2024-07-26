using System.Collections.Generic;
using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfSpine
    {
        internal static class Attributes
        {
            public static readonly XName Toc = "toc";
        }

        public string Toc { get; internal set; }
        public IList<OpfSpineItemRef> ItemRefs { get; internal set; } = new List<OpfSpineItemRef>();
    }
}
