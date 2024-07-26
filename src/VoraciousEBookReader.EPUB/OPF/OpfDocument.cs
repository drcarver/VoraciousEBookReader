using System.Linq;
using System.Xml.Linq;
using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfDocument
    {
        internal static class Attributes
        {
            public static readonly XName UniqueIdentifier = "unique-identifier";
            public static readonly XName Version = "version";
        }

        public string UniqueIdentifier { get; internal set; }
        public EPUBVersion EPUBVersion { get; internal set; }
        public OpfMetadata Metadata { get; internal set; } = new OpfMetadata();
        public OpfManifest Manifest { get; internal set; } = new OpfManifest();
        public OpfSpine Spine { get; internal set; } = new OpfSpine();
        public OpfGuide Guide { get; internal set; } = new OpfGuide();

        internal string FindCoverPath()
        {
            var coverMetaItem = Metadata.FindCoverMeta();
            if (coverMetaItem != null)
            {
                var item = Manifest.Items.FirstOrDefault(e => e.Id == coverMetaItem.Text);
                if (item != null)
                {
                    return item.Href;
                }
            }

            var coverItem = Manifest.FindCoverItem();
            return coverItem?.Href;
        }

        internal string FindAndRemoveCover()
        {
            var path = FindCoverPath();
            var meta = Metadata.FindAndDeleteCoverMeta();
            Manifest.DeleteCoverItem(meta?.Text);
            return path;
        }

        internal string FindNcxPath()
        {
            string path = null;

            var ncxItem = Manifest.Items.FirstOrDefault(e => e.MediaType == "application/x-dtbncx+xml");
            if (ncxItem != null)
            {
                path = ncxItem.Href;
            }
            else
            {
                // If we can't find the toc by media-type then try to look for id of the item in the spine attributes as
                // according to http://www.idpf.org/EPUB/20/spec/OPF_2.0.1_draft.htm#Section2.4.1.2,
                // "The item that describes the NCX must be referenced by the spine toc attribute."

                if (!string.IsNullOrWhiteSpace(Spine.Toc))
                {
                    var tocItem = Manifest.Items.FirstOrDefault(e => e.Id == Spine.Toc);
                    if (tocItem != null)
                    {
                        path = tocItem.Href;
                    }
                }
            }

            return path;
        }

        internal string FindNavPath()
        {
            var navItem = Manifest.Items.FirstOrDefault(e => e.Properties.Contains("nav"));
            return navItem?.Href;
        }
    }
}
