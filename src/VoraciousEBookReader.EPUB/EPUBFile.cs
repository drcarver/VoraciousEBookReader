using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB
{
    public abstract class EPUBFile
    {
        public string AbsolutePath { get; set; }
        public string Href { get; set; }
        public EPUBContentType ContentType { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }

        public override string ToString()
        {
            return $"{AbsolutePath} via HREF {Href}";
        }
    }
}
