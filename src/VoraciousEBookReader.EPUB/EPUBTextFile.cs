using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBTextFile : EPUBFile
    {
        public string TextContent
        {
            get { return Constants.DefaultEncoding.GetString(Content, 0, Content.Length); }
            set { Content = Constants.DefaultEncoding.GetBytes(value); }
        }
    }
}
