using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.Models
{
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Aliases { get; set; }
        public Relator PersonType { get; set; }
        public int BirthDate { get; set; }
        public int DeathDate { get; set; }
        public string? Webpage { get; set; }
        public string? BookBookId { get; set; }
    }
}
