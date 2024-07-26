using System.Collections.Generic;
using System.Linq;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfManifest
    {
        internal const string ManifestItemCoverImageProperty = "cover-image";

        public IList<OpfManifestItem> Items { get; internal set; } = new List<OpfManifestItem>();

        internal OpfManifestItem FindCoverItem()
        {
            return Items.FirstOrDefault(e => e.Properties.Contains(ManifestItemCoverImageProperty));
        }

        internal void DeleteCoverItem(string id = null)
        {
            var item = id != null ? Items.FirstOrDefault(e => e.Id == id) : FindCoverItem();
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }
}
