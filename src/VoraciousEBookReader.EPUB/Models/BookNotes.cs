using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoraciousEBookReader.EPUB.Models
{
    public class BookNotes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Book 
        public int BookId { get; set; }
        public virtual Book Books { get; set; }
    }
}
