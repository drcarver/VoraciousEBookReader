using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB.NCX
{
    public class NcxNavList
    {
        public string Id { get; internal set; }
        public string Class { get; internal set; }
        public string Label { get; internal set; }
        public IList<NcxNavTarget> NavTargets { get; internal set; } = new List<NcxNavTarget>();
    }
}
