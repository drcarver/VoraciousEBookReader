using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB.OPF
{
    public class OpfGuide
    {
        public IList<OpfGuideReference> References { get; internal set; } = new List<OpfGuideReference>();
    }
}
