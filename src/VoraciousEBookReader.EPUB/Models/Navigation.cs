using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.Models
{
    public class Navigation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? BookId { get; set; }
        public string? CurrSpot { get; set; }
        public UserStatus CurrStatus { get; set; }
        public DateTimeOffset FirstNavigationDate { get; set; }
        public DateTimeOffset MostRecentNavigationDate { get; set; }
        public int NCatalogViews { get; set; }
        public int NReading { get; set; }
        public int NSpecificSelection { get; set; }
        public int NSwipeLeft { get; set; }
        public int NSwipeRight { get; set; }
        public DateTimeOffset TimeMarkedDone { get; set; }
    }
}
