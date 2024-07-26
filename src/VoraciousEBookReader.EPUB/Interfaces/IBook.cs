using System.Collections.Generic;
using System.Collections.ObjectModel;

using VoraciousEBookReader.EPUB.Enum;
using VoraciousEBookReader.EPUB.Models;

namespace VoraciousEBookReader.EPUB.Interfaces
{
    public interface IBook
    {
        string BestAuthorDefaultIsNull { get; }
        string BookId { get; set; }
        string BookSeries { get; set; }
        string BookSource { get; set; }
        FileType BookType { get; set; }
        long DenormDownloadDate { get; set; }
        string DenormPrimaryAuthor { get; set; }
        string Description { get; set; }
        Download Download { get; set; }
        ObservableCollection<FilenameAndFormat> Files { get; set; }
        string Imprint { get; set; }
        string Issued { get; set; }
        string Language { get; set; }
        string LCC { get; set; }
        string LCCN { get; set; }
        string LCSH { get; set; }
        Navigation Navigation { get; set; }
        BookNotes Notes { get; set; }
        ObservableCollection<Person> People { get; set; }
        string PGEditionInfo { get; set; }
        string PGNotes { get; set; }
        string PGProducedBy { get; set; }
        UserReview Review { get; set; }
        string Title { get; set; }
        string TitleAlternative { get; set; }

        string GetBestTitleForFilename();
        IList<string> GetSearchArea(string inputArea);
        string ToString();
        string Validate();
    }
}