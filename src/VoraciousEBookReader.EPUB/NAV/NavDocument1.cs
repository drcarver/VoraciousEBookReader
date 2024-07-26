namespace VoraciousEBookReader.EPUB.NAV
{
    public class NavDocument
    {
        public NavHead Head { get; internal set; } = new NavHead();
        public NavBody Body { get; internal set; } = new NavBody();
    }
}
