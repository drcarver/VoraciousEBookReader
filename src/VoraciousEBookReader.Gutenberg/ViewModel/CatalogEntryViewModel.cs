using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.Model;

namespace VoraciousEBookReader.Gutenberg.ViewModel
{
    public partial class CatalogEntryViewModel : ObservableObject, IGutenbergCatalogEntry
    {
        [ObservableProperty]
        private string authors;

        [ObservableProperty]
        private string bookshelves;

        [ObservableProperty]
        private int ebookNumber;

        [ObservableProperty]
        private string ePubType;

        [ObservableProperty]
        private DateTime issued;

        [ObservableProperty]
        private string language;

        [ObservableProperty]
        private string loCC;

        [ObservableProperty]
        private string subjects;

        [ObservableProperty]
        private string title;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entry">The catalog entry to create</param>
        public CatalogEntryViewModel(GutenbergCatalogEntry entry)
        {
            Authors = entry.Authors;
            Bookshelves = entry.Bookshelves;
            EbookNumber = entry.EbookNumber;
            EPubType = entry.EPubType;
            Issued = entry.Issued;
            Language = entry.Language;
            LoCC = entry.LoCC;
            Subjects = entry.Subjects;
            Title = entry.Title;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogEntryViewModel()
        {
        }
    }
}
