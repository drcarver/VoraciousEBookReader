using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.Models
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BookId { get; set; }
        public string? BookSource { get; set; }
        public FileType BookType { get; set; }
        public string? Description { get; set; }
        public string? Imprint { get; set; }
        public string? Issued { get; set; }
        public string? Title { get; set; }
        public string? TitleAlternative { get; set; }
        public string? Language { get; set; }
        public string? LCSH { get; set; }
        public string? LCCN { get; set; }
        public string? PGEditionInfo { get; set; }
        public string? PGProducedBy { get; set; }
        public string? PGNotes { get; set; }
        public string? BookSeries { get; set; }
        public string? LCC { get; set; }
        public string? DenormPrimaryAuthor { get; set; }
        public long DenormDownloadDate { get; set; }

        // The notes Id
        public int? BookNotesId { get; set; }
        public virtual BookNotes? BookNotes { get; set; }

        // The Review Id
        public int? UserReviewId { get; set; }
        public virtual UserReview? UserReview { get; set; }

        // The download Id
        public int? DownloadId { get; set; }
        public virtual Download? Download { get; set; }

        // The download Id
        public int? NavigationId { get; set; }
        public virtual Navigation? Navigation { get; set; }
    }
}
