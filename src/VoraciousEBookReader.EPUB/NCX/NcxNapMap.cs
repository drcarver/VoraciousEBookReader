using System.Collections.Generic;
using System.Xml.Linq;

namespace VoraciousEBookReader.EPUB.NCX
{
    public class NcxNapMap
    {
        /// <summary>
        /// Populated only when an EPUB with NCX is read.
        /// </summary>
        public XElement Dom { get; internal set; }
        public IList<NcxNavPoint> NavPoints { get; internal set; } = new List<NcxNavPoint>();
    }
}
