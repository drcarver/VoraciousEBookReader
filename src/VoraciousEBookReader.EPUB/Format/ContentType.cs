using System.Collections.Generic;
using System.Linq;

using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.Enum
{
    internal class ContentType
    {
        public static readonly IReadOnlyDictionary<string, EPUBContentType> MimeTypeToContentType = new Dictionary<string, EPUBContentType>
        {
            { "application/xhtml+xml", EPUBContentType.Xhtml11 },
            { "application/x-dtbook+xml", EPUBContentType.Dtbook },
            { "application/x-dtbncx+xml", EPUBContentType.DtbookNcx },
            { "text/x-oeb1-document", EPUBContentType.Oeb1Document },
            { "application/xml", EPUBContentType.Xml },
            { "text/css", EPUBContentType.Css },
            { "text/x-oeb1-css", EPUBContentType.Oeb1Css },
            { "image/gif", EPUBContentType.ImageGif },
            { "image/jpeg", EPUBContentType.ImageJpeg },
            { "image/png", EPUBContentType.ImagePng },
            { "image/svg+xml", EPUBContentType.ImageSvg },
            { "font/truetype", EPUBContentType.FontTruetype },
            { "font/opentype", EPUBContentType.FontOpentype },
            { "application/vnd.ms-opentype", EPUBContentType.FontOpentype }
        };

        public static readonly IReadOnlyDictionary<EPUBContentType, string> ContentTypeToMimeType = MimeTypeToContentType
            .Where(pair => pair.Key != "application/vnd.ms-opentype") // Because it's defined twice.
            .ToDictionary(pair => pair.Value, pair => pair.Key);
    }
}
