using System.Collections.Generic;
using System.Linq;
using System.Text;

using VoraciousEBookReader.EPUB.Misc;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBBook
    {
        internal const string AuthorsSeparator = ", ";

        /// <summary>
        /// Read-only raw EPUB format structures.
        /// </summary>
        public EPUBFormat Format { get; internal set; }

        public string Title => Format.Opf.Metadata.Titles.FirstOrDefault();

        public IEnumerable<string> Authors => Format.Opf.Metadata.Creators.Select(creator => creator.Text);

        /// <summary>
        /// All files within the EPUB.
        /// </summary>
        public EPUBResources Resources { get; internal set; }

        /// <summary>
        /// EPUB format specific resources.
        /// </summary>
        public EPUBSpecialResources SpecialResources { get; internal set; }

        public byte[] CoverImage { get; internal set; }

        public IList<EPUBChapter> TableOfContents { get; internal set; }

        public string ToPlainText()
        {
            var builder = new StringBuilder();
            foreach (var html in SpecialResources.HtmlInReadingOrder)
            {
                builder.Append(Html.GetContentAsPlainText(html.TextContent));
                builder.Append('\n');
            }
            return builder.ToString().Trim();
        }
    }
}
