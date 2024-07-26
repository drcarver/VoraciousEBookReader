using System.Collections.Generic;
using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfSpineItemRef
    {
        internal static class Attributes
        {
            public static readonly XName IdRef = "idref";
            public static readonly XName Linear = "linear";
            public static readonly XName Id = "id";
            public static readonly XName Properties = "properties";
        }

        public string IdRef { get; internal set; }
        public bool Linear { get; internal set; }
        public string Id { get; internal set; }
        public IList<string> Properties { get; internal set; } = new List<string>();

        public override string ToString()
        {
            return "IdRef: " + IdRef;
        }
    }
}
