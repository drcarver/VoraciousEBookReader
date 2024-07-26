using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using VoraciousEBookReader.EPUB.Enum;
using VoraciousEBookReader.EPUB.Extensions;
using VoraciousEBookReader.EPUB.Format;
using VoraciousEBookReader.EPUB.Misc;
using VoraciousEBookReader.EPUB.NAV;
using VoraciousEBookReader.EPUB.NCX;
using VoraciousEBookReader.EPUB.OCF;
using VoraciousEBookReader.EPUB.OPF;

namespace VoraciousEBookReader.EPUB
{
    public static class EPUBReader
    {
        public static EPUBBook Read(string filePath, Encoding encoding = null)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (encoding == null) encoding = Constants.DefaultEncoding;

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Specified EPUB file not found.", filePath);
            }

            return Read(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read), false, encoding);
        }

        public static EPUBBook Read(byte[] EPUBData, Encoding encoding = null)
        {
            if (encoding == null) encoding = Constants.DefaultEncoding;
            return Read(new MemoryStream(EPUBData), false, encoding);
        }

        public static EPUBBook Read(Stream stream, bool leaveOpen, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (encoding == null) encoding = Constants.DefaultEncoding;

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen, encoding))
            {
                var format = new EPUBFormat { Ocf = OcfReader.Read(archive.LoadXml(Constants.OcfPath)) };

                format.Paths.OcfAbsolutePath = Constants.OcfPath;

                format.Paths.OpfAbsolutePath = format.Ocf.RootFilePath;
                if (format.Paths.OpfAbsolutePath == null)
                {
                    throw new EPUBParseException("EPUB OCF doesn't specify a root file.");
                }

                format.Opf = OpfReader.Read(archive.LoadXml(format.Paths.OpfAbsolutePath));

                var navPath = format.Opf.FindNavPath();
                if (navPath != null)
                {
                    format.Paths.NavAbsolutePath = navPath.ToAbsolutePath(format.Paths.OpfAbsolutePath);
                    format.Nav = NavReader.Read(archive.LoadHtml(format.Paths.NavAbsolutePath));
                }

                var ncxPath = format.Opf.FindNcxPath();
                if (ncxPath != null)
                {
                    format.Paths.NcxAbsolutePath = ncxPath.ToAbsolutePath(format.Paths.OpfAbsolutePath);
                    format.Ncx = NcxReader.Read(archive.LoadXml(format.Paths.NcxAbsolutePath));
                }

                var book = new EPUBBook { Format = format };
                book.Resources = LoadResources(archive, book);
                book.SpecialResources = LoadSpecialResources(archive, book);
                book.CoverImage = LoadCoverImage(book);
                book.TableOfContents = LoadChapters(book);
                return book;
            }
        }

        private static byte[] LoadCoverImage(EPUBBook book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            if (book.Format == null) throw new ArgumentNullException(nameof(book.Format));

            var coverPath = book.Format.Opf.FindCoverPath();
            if (coverPath == null)
            {
                return null;
            }

            var coverImageFile = book.Resources.Images.SingleOrDefault(e => e.Href == coverPath);
            return coverImageFile?.Content;
        }

        private static List<EPUBChapter> LoadChapters(EPUBBook book)
        {
            if (book.Format.Nav != null)
            {
                var tocNav = book.Format.Nav.Body.Navs.SingleOrDefault(e => e.Type == NavNav.Attributes.TypeValues.Toc);
                if (tocNav != null)
                {
                    return LoadChaptersFromNav(book.Format.Paths.NavAbsolutePath, tocNav.Dom);
                }
            }

            if (book.Format.Ncx != null)
            {
                return LoadChaptersFromNcx(book.Format.Paths.NcxAbsolutePath, book.Format.Ncx.NavMap.NavPoints);
            }

            return new List<EPUBChapter>();
        }

        private static List<EPUBChapter> LoadChaptersFromNav(string navAbsolutePath, XElement element, EPUBChapter parentChapter = null)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var ns = element.Name.Namespace;

            var result = new List<EPUBChapter>();
            var previous = parentChapter;

            var ol = element.Element(ns + NavElements.Ol);
            if (ol == null)
                return result;

            foreach (var li in ol.Elements(ns + NavElements.Li))
            {
                var chapter = new EPUBChapter
                {
                    Parent = parentChapter,
                    Previous = previous
                };

                if (previous != null)
                    previous.Next = chapter;

                var link = li.Element(ns + NavElements.A);
                if (link != null)
                {
                    var id = link.Attribute("id")?.Value;
                    if (id != null)
                    {
                        chapter.Id = id;
                    }

                    var url = link.Attribute("href")?.Value;
                    if (url != null)
                    {
                        var href = new Href(url);
                        chapter.RelativePath = href.Path;
                        chapter.HashLocation = href.HashLocation;
                        chapter.AbsolutePath = chapter.RelativePath.ToAbsolutePath(navAbsolutePath);
                    }

                    var titleTextElement = li.Descendants().FirstOrDefault(e => !string.IsNullOrWhiteSpace(e.Value));
                    if (titleTextElement != null)
                    {
                        chapter.Title = titleTextElement.Value;
                    }

                    if (li.Element(ns + NavElements.Ol) != null)
                    {
                        chapter.SubChapters = LoadChaptersFromNav(navAbsolutePath, li, chapter);
                    }
                    result.Add(chapter);

                    previous = chapter.SubChapters.Any() ? chapter.SubChapters.Last() : chapter;
                }
            }

            return result;
        }

        private static List<EPUBChapter> LoadChaptersFromNcx(string ncxAbsolutePath, IEnumerable<NcxNavPoint> navigationPoints, EPUBChapter parentChapter = null)
        {
            var result = new List<EPUBChapter>();
            var previous = parentChapter;

            foreach (var navigationPoint in navigationPoints)
            {
                var chapter = new EPUBChapter
                {
                    Title = navigationPoint.NavLabelText,
                    Parent = parentChapter,
                    Previous = previous
                };

                if (previous != null)
                    previous.Next = chapter;

                var href = new Href(navigationPoint.ContentSrc);
                chapter.RelativePath = href.Path;
                chapter.AbsolutePath = href.Path.ToAbsolutePath(ncxAbsolutePath);
                chapter.HashLocation = href.HashLocation;
                chapter.SubChapters = LoadChaptersFromNcx(ncxAbsolutePath, navigationPoint.NavPoints, chapter);
                result.Add(chapter);

                previous = chapter.SubChapters.Any() ? chapter.SubChapters.Last() : chapter;
            }
            return result;
        }

        private static EPUBResources LoadResources(ZipArchive EPUBArchive, EPUBBook book)
        {
            var resources = new EPUBResources();

            foreach (var item in book.Format.Opf.Manifest.Items)
            {
                var path = item.Href.ToAbsolutePath(book.Format.Paths.OpfAbsolutePath);
                var entry = EPUBArchive.GetEntryImproved(path);

                if (entry == null)
                {
                    throw new EPUBParseException($"file {path} not found in archive.");
                }
                if (entry.Length > int.MaxValue)
                {
                    throw new EPUBParseException($"file {path} is bigger than 2 Gb.");
                }

                var href = item.Href;
                var mimeType = item.MediaType;

                EPUBContentType contentType;
                contentType = ContentType.MimeTypeToContentType.TryGetValue(mimeType, out contentType)
                    ? contentType
                    : EPUBContentType.Other;

                switch (contentType)
                {
                    case EPUBContentType.Xhtml11:
                    case EPUBContentType.Css:
                    case EPUBContentType.Oeb1Document:
                    case EPUBContentType.Oeb1Css:
                    case EPUBContentType.Xml:
                    case EPUBContentType.Dtbook:
                    case EPUBContentType.DtbookNcx:
                        {
                            var file = new EPUBTextFile
                            {
                                AbsolutePath = path,
                                Href = href,
                                MimeType = mimeType,
                                ContentType = contentType
                            };

                            resources.All.Add(file);

                            using (var stream = entry.Open())
                            {
                                file.Content = stream.ReadToEnd();
                            }

                            switch (contentType)
                            {
                                case EPUBContentType.Xhtml11:
                                    resources.Html.Add(file);
                                    break;
                                case EPUBContentType.Css:
                                    resources.Css.Add(file);
                                    break;
                                default:
                                    resources.Other.Add(file);
                                    break;
                            }
                            break;
                        }
                    default:
                        {
                            var file = new EPUBByteFile
                            {
                                AbsolutePath = path,
                                Href = href,
                                MimeType = mimeType,
                                ContentType = contentType
                            };

                            resources.All.Add(file);

                            using (var stream = entry.Open())
                            {
                                if (stream == null)
                                {
                                    throw new EPUBException($"Incorrect EPUB file: content file \"{href}\" specified in manifest is not found");
                                }

                                using (var memoryStream = new MemoryStream((int)entry.Length))
                                {
                                    stream.CopyTo(memoryStream);
                                    file.Content = memoryStream.ToArray();
                                }
                            }

                            switch (contentType)
                            {
                                case EPUBContentType.ImageGif:
                                case EPUBContentType.ImageJpeg:
                                case EPUBContentType.ImagePng:
                                case EPUBContentType.ImageSvg:
                                    resources.Images.Add(file);
                                    break;
                                case EPUBContentType.FontTruetype:
                                case EPUBContentType.FontOpentype:
                                    resources.Fonts.Add(file);
                                    break;
                                default:
                                    resources.Other.Add(file);
                                    break;
                            }
                            break;
                        }
                }
            }

            return resources;
        }

        private static EPUBSpecialResources LoadSpecialResources(ZipArchive EPUBArchive, EPUBBook book)
        {
            var result = new EPUBSpecialResources
            {
                Ocf = new EPUBTextFile
                {
                    AbsolutePath = Constants.OcfPath,
                    Href = Constants.OcfPath,
                    ContentType = EPUBContentType.Xml,
                    MimeType = ContentType.ContentTypeToMimeType[EPUBContentType.Xml],
                    Content = EPUBArchive.LoadBytes(Constants.OcfPath)
                },
                Opf = new EPUBTextFile
                {
                    AbsolutePath = book.Format.Paths.OpfAbsolutePath,
                    Href = book.Format.Paths.OpfAbsolutePath,
                    ContentType = EPUBContentType.Xml,
                    MimeType = ContentType.ContentTypeToMimeType[EPUBContentType.Xml],
                    Content = EPUBArchive.LoadBytes(book.Format.Paths.OpfAbsolutePath)
                },
                HtmlInReadingOrder = new List<EPUBTextFile>()
            };

            var htmlFiles = book.Format.Opf.Manifest.Items
                .Where(item => ContentType.MimeTypeToContentType.ContainsKey(item.MediaType) && ContentType.MimeTypeToContentType[item.MediaType] == EPUBContentType.Xhtml11)
                .ToDictionary(item => item.Id, item => item.Href);

            foreach (var item in book.Format.Opf.Spine.ItemRefs)
            {
                if (!htmlFiles.TryGetValue(item.IdRef, out string href))
                {
                    continue;
                }

                var html = book.Resources.Html.SingleOrDefault(e => e.Href == href);
                if (html != null)
                {
                    result.HtmlInReadingOrder.Add(html);
                }
            }

            return result;
        }
    }
}
