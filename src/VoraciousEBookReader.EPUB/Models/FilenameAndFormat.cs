using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Text;

namespace VoraciousEBookReader.EPUB.Models
{
    public class FilenameAndFormat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string? BookId { get; set; }
        public int Extent { get; set; }
        public string? MimeType { get; set; }
        public string? BookBookId { get; set; }
    }
}
