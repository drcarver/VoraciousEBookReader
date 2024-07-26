using System.Collections.Generic;
using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.NAV
{
    public class NavHead
    {
        /// <summary>
        /// Instantiated only when the EPUB was read.
        /// </summary>
        internal XElement Dom { get; set; }

        public string Title { get; internal set; }
        public IList<NavHeadLink> Links { get; internal set; } = new List<NavHeadLink>();
        public IList<NavMeta> Metas { get; internal set; } = new List<NavMeta>();
    }
}
