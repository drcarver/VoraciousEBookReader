using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBChapter
    {
        public string Id { get; set; }
        public string AbsolutePath { get; set; }
        public string RelativePath { get; set; }
        public string HashLocation { get; set; }
        public string Title { get; set; }

        public EPUBChapter Parent { get; set; }
        public EPUBChapter Previous { get; set; }
        public EPUBChapter Next { get; set; }
        public IList<EPUBChapter> SubChapters { get; set; } = new List<EPUBChapter>();

        public override string ToString()
        {
            return $"Title: {Title}, Subchapter count: {SubChapters.Count}";
        }
    }
}
