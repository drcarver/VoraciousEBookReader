using CsvHelper.Configuration;

using VoraciousEBookReader.Gutenberg.Interface;
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
            Map(x => x.BookNumber).Name("Text#");
            Map(x => x.BookEPubType).Name("Type");
            Map(x => x.BookDateIssued).Name("Issued");
            Map(x => x.BookTitle).Name("Title");
            Map(x => x.BookLOCC).Name("LoCC");
            Map(x => x.Language).Name("Language");
            Map(x => x.Author).Name("Authors");
            Map(x => x.Subject).Name("Subjects");
            Map(x => x.Shelf).Name("Bookshelves");
        }
    }
}
