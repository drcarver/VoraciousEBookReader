using System;

using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.Gutenberg.Model
{
    /// <summary>
    /// The model for the Gutenberg catalog
    /// </summary>
    public class GutenbergCatalogEntry : IGutenbergCatalogEntry
    {
        public int EbookNumber { get; set; }
        public string EPubType { get; set; }
        public DateTime Issued { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Authors { get; set; }
        public string Subjects { get; set; }
        public string LoCC { get; set; }
        public string Bookshelves { get; set; }
    }
}
