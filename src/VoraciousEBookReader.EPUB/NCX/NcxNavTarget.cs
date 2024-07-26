namespace VoraciousEBookReader.EPUB.NCX
{
    public class NcxNavTarget
    {
        public string Id { get; internal set; }
        public string Class { get; internal set; }
        public int? PlayOrder { get; internal set; }
        public string Label { get; internal set; }
        public string ContentSource { get; internal set; }
    }
}
