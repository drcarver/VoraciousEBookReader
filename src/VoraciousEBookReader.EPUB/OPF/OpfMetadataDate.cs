using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfMetadataDate
    {
        internal static class Attributes
        {
            public static readonly XName Event = Constants.OpfNamespace + "event";
        }

        public string Text { get; internal set; }

        /// <summary>
        /// i.e. "modification"
        /// </summary>
        public string Event { get; internal set; }
    }
}
