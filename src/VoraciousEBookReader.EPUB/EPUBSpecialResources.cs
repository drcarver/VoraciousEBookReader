using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBSpecialResources
    {
        public EPUBTextFile Ocf { get; internal set; }
        public EPUBTextFile Opf { get; internal set; }
        public IList<EPUBTextFile> HtmlInReadingOrder { get; internal set; } = new List<EPUBTextFile>();
    }
}
