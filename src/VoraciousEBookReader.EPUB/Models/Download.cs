using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.Models
{
    public class Download
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? BookId { get; set; }
        public FileStatus CurrFileStatus { get; set; }
        public DateTimeOffset DownloadDate { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }
}
