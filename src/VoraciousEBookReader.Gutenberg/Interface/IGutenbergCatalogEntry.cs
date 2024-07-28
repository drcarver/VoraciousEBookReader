using System;

namespace VoraciousEBookReader.Gutenberg.Interface
{
    public interface IGutenbergCatalogEntry
    {
        string Authors { get; set; }
        string Bookshelves { get; set; }
        int EbookNumber { get; set; }
        string EPubType { get; set; }
        DateTime Issued { get; set; }
        string Language { get; set; }
        string LoCC { get; set; }
        string Subjects { get; set; }
        string Title { get; set; }
    }
}