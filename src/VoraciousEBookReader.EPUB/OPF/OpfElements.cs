using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.OPF
{
    internal static class OpfElements
    {
        public static readonly XName Package = Constants.OpfNamespace + "package";

        public static readonly XName Metadata = Constants.OpfNamespace + "metadata";
        public static readonly XName Contributor = Constants.OpfMetadataNamespace + "contributor";
        public static readonly XName Coverages = Constants.OpfMetadataNamespace + "coverages";
        public static readonly XName Creator = Constants.OpfMetadataNamespace + "creator";
        public static readonly XName Date = Constants.OpfMetadataNamespace + "date";
        public static readonly XName Description = Constants.OpfMetadataNamespace + "description";
        public static readonly XName Format = Constants.OpfMetadataNamespace + "format";
        public static readonly XName Identifier = Constants.OpfMetadataNamespace + "identifier";
        public static readonly XName Language = Constants.OpfMetadataNamespace + "language";
        public static readonly XName Meta = Constants.OpfNamespace + "meta";
        public static readonly XName Publisher = Constants.OpfMetadataNamespace + "publisher";
        public static readonly XName Relation = Constants.OpfMetadataNamespace + "relation";
        public static readonly XName Rights = Constants.OpfMetadataNamespace + "rights";
        public static readonly XName Source = Constants.OpfMetadataNamespace + "source";
        public static readonly XName Subject = Constants.OpfMetadataNamespace + "subject";
        public static readonly XName Title = Constants.OpfMetadataNamespace + "title";
        public static readonly XName Type = Constants.OpfMetadataNamespace + "type";

        public static readonly XName Guide = Constants.OpfNamespace + "guide";
        public static readonly XName Reference = Constants.OpfNamespace + "reference";

        public static readonly XName Manifest = Constants.OpfNamespace + "manifest";
        public static readonly XName Item = Constants.OpfNamespace + "item";

        public static readonly XName Spine = Constants.OpfNamespace + "spine";
        public static readonly XName ItemRef = Constants.OpfNamespace + "itemref";
    }
}
