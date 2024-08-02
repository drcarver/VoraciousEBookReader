using CsvHelper.Configuration;
using VoraciousEBookReader.Gutenberg.Model;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Map
{
    /// <summary>
    /// The CSV map for the Gutenberg catalog
    /// </summary>
    internal sealed class GutenbergCatalogEntryViewModelMap : ClassMap<GutenbergCatalogEntryViewModel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal GutenbergCatalogEntryViewModelMap()
        {
            Map(x => x.EbookNumber).Name("Text#");
            Map(x => x.EPubType).Name("Type");
            Map(x => x.Issued).Name("Issued");
            Map(x => x.Title).Name("Title");
            Map(x => x.Language).Name("Language");
            Map(x => x.Authors).Name("Authors");
            Map(x => x.Subjects).Name("Subjects");
            Map(x => x.LoCC).Name("LoCC");
            Map(x => x.Bookshelves).Name("Bookshelves");
        }
    }
}
