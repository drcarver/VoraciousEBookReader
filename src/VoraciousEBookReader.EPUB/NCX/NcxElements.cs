using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.NCX
{
    internal static class NcxElements
    {
        public static readonly XName Ncx = Constants.NcxNamespace + "ncx";
        public static readonly XName Head = Constants.NcxNamespace + "head";
        public static readonly XName Meta = Constants.NcxNamespace + "meta";
        public static readonly XName DocTitle = Constants.NcxNamespace + "docTitle";
        public static readonly XName DocAuthor = Constants.NcxNamespace + "docAuthor";
        public static readonly XName Text = Constants.NcxNamespace + "text";
        public static readonly XName NavMap = Constants.NcxNamespace + "navMap";
        public static readonly XName NavPoint = Constants.NcxNamespace + "navPoint";
        public static readonly XName NavList = Constants.NcxNamespace + "navList";
        public static readonly XName PageList = Constants.NcxNamespace + "pageList";
        public static readonly XName NavInfo = Constants.NcxNamespace + "navInfo";
        public static readonly XName PageTarget = Constants.NcxNamespace + "pageTarget";
        public static readonly XName NavLabel = Constants.NcxNamespace + "navLabel";
        public static readonly XName NavTarget = Constants.NcxNamespace + "navTarget";
        public static readonly XName Content = Constants.NcxNamespace + "content";
    }
}
