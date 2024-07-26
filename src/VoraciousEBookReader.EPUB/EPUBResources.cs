using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBResources
    {
        public IList<EPUBTextFile> Html { get; internal set; } = new List<EPUBTextFile>();
        public IList<EPUBTextFile> Css { get; internal set; } = new List<EPUBTextFile>();
        public IList<EPUBByteFile> Images { get; internal set; } = new List<EPUBByteFile>();
        public IList<EPUBByteFile> Fonts { get; internal set; } = new List<EPUBByteFile>();
        public IList<EPUBFile> Other { get; internal set; } = new List<EPUBFile>();

        /// <summary>
        /// This is a concatenation of all the resources files in the EPUB: 
        /// html, css, images, etc.
        /// </summary>
        public IList<EPUBFile> All { get; internal set; } = new List<EPUBFile>();
    }
}
