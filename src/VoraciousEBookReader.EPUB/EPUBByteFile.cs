namespace VoraciousEBookReader.EPUB
{
    public class EPUBByteFile : EPUBFile
    {
        internal EPUBTextFile ToTextFile()
        {
            return new EPUBTextFile
            {
                Content = Content,
                ContentType = ContentType,
                AbsolutePath = AbsolutePath,
                Href = Href,
                MimeType = MimeType
            };
        }
    }
}
