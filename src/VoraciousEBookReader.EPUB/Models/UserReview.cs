using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoraciousEBookReader.EPUB.Models
{
    public class UserReview
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? BookId { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset MostRecentModificationDate { get; set; }
        public double NStars { get; set; }
        public string? Review { get; set; }
        public string? Tags { get; set; }
    }
}
