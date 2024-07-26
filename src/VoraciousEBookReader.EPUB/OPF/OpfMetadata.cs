using System.Collections.Generic;
using System.Linq;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfMetadata
    {
        public IList<string> Titles { get; internal set; } = new List<string>();
        public IList<string> Subjects { get; internal set; } = new List<string>();
        public IList<string> Descriptions { get; internal set; } = new List<string>();
        public IList<string> Publishers { get; internal set; } = new List<string>();
        public IList<OpfMetadataCreator> Creators { get; internal set; } = new List<OpfMetadataCreator>();
        public IList<OpfMetadataCreator> Contributors { get; internal set; } = new List<OpfMetadataCreator>();
        public IList<OpfMetadataDate> Dates { get; internal set; } = new List<OpfMetadataDate>();
        public IList<string> Types { get; internal set; } = new List<string>();
        public IList<string> Formats { get; internal set; } = new List<string>();
        public IList<OpfMetadataIdentifier> Identifiers { get; internal set; } = new List<OpfMetadataIdentifier>();
        public IList<string> Sources { get; internal set; } = new List<string>();
        public IList<string> Languages { get; internal set; } = new List<string>();
        public IList<string> Relations { get; internal set; } = new List<string>();
        public IList<string> Coverages { get; internal set; } = new List<string>();
        public IList<string> Rights { get; internal set; } = new List<string>();
        public IList<OpfMetadataMeta> Metas { get; internal set; } = new List<OpfMetadataMeta>();

        internal OpfMetadataMeta FindCoverMeta()
        {
            return Metas.FirstOrDefault(metaItem => metaItem.Name == "cover");
        }

        internal OpfMetadataMeta FindAndDeleteCoverMeta()
        {
            var meta = FindCoverMeta();
            if (meta == null) return null;
            Metas.Remove(meta);
            return meta;
        }
    }
}
