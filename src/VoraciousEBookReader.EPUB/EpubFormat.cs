using VoraciousEBookReader.EPUB.NAV;
using VoraciousEBookReader.EPUB.NCX;
using VoraciousEBookReader.EPUB.OCF;
using VoraciousEBookReader.EPUB.OPF;

namespace VoraciousEBookReader.EPUB
{

    public class EPUBFormat
    {
        public EPUBFormatPaths Paths { get; internal set; } = new EPUBFormatPaths();

        public OcfDocument Ocf { get; internal set; }
        public OpfDocument Opf { get; internal set; }
        public NcxDocument Ncx { get; internal set; }
        public NavDocument Nav { get; internal set; }
    }
}
