using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB.NCX
{
    /// <summary>
    /// DAISY’s Navigation Center eXtended (NCX)
    /// </summary>
    public class NcxDocument
    {
        public IList<NcxMeta> Meta { get; internal set; } = new List<NcxMeta>();
        public string DocTitle { get; internal set; }
        public string DocAuthor { get; internal set; }
        public NcxNapMap NavMap { get; internal set; } = new NcxNapMap(); // <navMap> is a required element in NCX.
        public NcxPageList PageList { get; internal set; }
        public NcxNavList NavList { get; internal set; }
    }
}
