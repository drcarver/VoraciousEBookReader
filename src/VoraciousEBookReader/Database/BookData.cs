using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;
using VoraciousEBookReader.EPUB.Enum;


#if IS_MIGRATION_PROJECT
// Just dummy this up for the migration project.
interface IGetSearchArea { }
#else
using SimpleEPUBReader.Searching;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Diagnostics;

using static SimpleEPUBReader.Database.Person;
#endif

namespace SimpleEPUBReader.Database
{
    public partial class BooksContext : DbContext
    {
        public static string BooksDatabaseFilename = "Books.db";
        public DbSet<Books> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
#if IS_MIGRATION_PROJECT
            // EF (Entity Framework Core) requires that we use a "migration" to 
            // make the database tables. The migration can only be done in a 
            // console project, so my solution needs a console project whose only
            // reason in life is to be the thing from which a migration can be
            // created.
            // As a console project, it doesn't have acccess to the local folder, etc.
            //
            // IS_MIGRATION_PROJECT is a value I defined and put into the build settings
            // for the console project.
            var path = "."; 
#else
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var path = folder.Path;
            folder.CreateFileAsync(BooksDatabaseFilename, Windows.Storage.CreationCollisionOption.OpenIfExists).AsTask().Wait();
#endif
            string dbpath = Path.Combine(path, BooksDatabaseFilename);
            options.UseSqlite($"Data Source={dbpath}");
        }

        /// <summary>
        /// See https://docs.microsoft.com/en-us/ef/core/platforms/#universal-windows-platform for why this is more performant
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Only do it this way when using the main database. When creating a new
            // database, use the default.
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotifications);
        }

        static BooksContext DbContextSingleton = null;

        public static BooksContext Get([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            // System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: Books: Get called from {memberName}");
            if (DbContextSingleton == null)
            {
                DbContextSingleton = new BooksContext();
            }
            return DbContextSingleton;
        }

        public static void ResetSingleton(string newName)
        {
            if (DbContextSingleton != null)
            {
                DbContextSingleton.SaveChanges();
                DbContextSingleton = null;
            }
            BooksDatabaseFilename = string.IsNullOrEmpty(newName) ? "Books.db" : newName;
        }
        
        public const string BookTypeGutenberg = "gutenberg.org";
        
        public const string BookTypeUser = "User-imported";

        public void DoMigration()
        {
            //SLOW: the migration is 3 seconds. This is probably the EF+DB startup time.
            this.Database.Migrate();
            Logger.Log($"Books:done migration");
            this.SaveChanges();
            Logger.Log($"Books:done save changes");
        }
    }

    public class FilenameAndFormatData : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public FilenameAndFormatData()
        {
            ;
        }
        public FilenameAndFormatData(FilenameAndFormatData source)
        {
            this.Id = source.Id;
            this.BookId = source.BookId;
            this.Extent = source.Extent;
            this.FileName = source.FileName;
            this.FileType = source.FileType;
            this.LastModified = source.LastModified;
            this.MimeType = source.MimeType;
        }

        private int id;
        private string fileName = "";
        private string fileType = "";
        private string lastModified = "";
        private string bookId = "";
        private int extent = -1;
        private string mimeType = "";

        /// <summary>
        /// The files are the variants of an ebook (plus ancilary stuff like title pages).
        /// Given a list of possible files, return an ordered list of the most appropriate
        /// files for the book, filtering to remove extra ones. For examples, if there's an
        /// epub with images and an epub without images, only include the epub with images.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IList<FilenameAndFormatData> GetProcessedFileList(IList<FilenameAndFormatData> start)
        {
            // For example: if there's an epub with images, don't include the epub without images.
            // If there's a high-res cover, don't include a low-res cover.
            // If there are any text files at all, don't include HTML.
            // Assumes that the original list is pretty random (which seems to be the case
            // for the XML data) and that the order doesn't matter. This is probably OK
            // because I don't expect, e.g., multiple conflicting files like two different
            // text versions.
            // FAIL: actually, the audio books includes a bazillion OGG etc files.

            var sortedlist = new List<FilenameAndFormatData>();
            var retval = new List<FilenameAndFormatData>();

            foreach (var item in start) sortedlist.Add(item);
            sortedlist.Sort((a, b) => { return a.GetFileType().CompareTo(b.GetFileType()); });
            bool haveEpub = false;
            bool haveCover = false;
            bool haveHtml = false;
            bool haveText = false;

            // Step one: figure out what we've got.
            foreach (var item in sortedlist)
            {
                var itemtype = item.GetFileType();
                switch (itemtype)
                {
                    case ProcessedFileType.CoverMedium:
                    case ProcessedFileType.CoverSmall:
                        if (!haveCover)
                        {
                            retval.Add(item);
                            haveCover = true;
                        }
                        break;
                    case ProcessedFileType.EPub:
                    case ProcessedFileType.EPubNoImages:
                        if (!haveEpub)
                        {
                            retval.Add(item);
                            haveEpub = true;
                        }
                        break;
                    case ProcessedFileType.PDF:
                    case ProcessedFileType.PS:
                    case ProcessedFileType.Tex:
                        // Only include the PDF/PS if we don't have an epub. Although some people
                        // might prefer a PDF, the Gutenberg reality is that they create epubs,
                        // not pdfs. The few cases with PDFs are a total anomoly.
                        if (!haveEpub)
                        {
                            retval.Add(item);
                        }
                        break;
                    case ProcessedFileType.RDF:
                        retval.Add(item);
                        break;
                    case ProcessedFileType.Text:
                    case ProcessedFileType.TextNotUtf8:
                        if (!haveText)
                        {
                            retval.Add(item);
                            haveText = true;
                        }
                        break;
                    case ProcessedFileType.Html:
                    case ProcessedFileType.HtmlNotUtf8:
                        if (!haveHtml) // Only inlude HTML if we don't have epub. And only include one.
                        {
                            if (!haveEpub)
                            {
                                retval.Add(item);
                            }
                            haveHtml = true;
                        }
                        break;

                    case ProcessedFileType.MobiPocket:
                    case ProcessedFileType.Unknown:
                        if (!haveEpub)
                        {
                            retval.Add(item);
                        }
                        break;
                }
            }

            return retval;
        }

        // Book can't be the primary key because there are duplicates. Use a synthasized Id
        // which will be maintained by the database.
        public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }
        public string FileName { get => fileName; set { if (fileName != value) { NotifyPropertyChanging(); fileName = value; NotifyPropertyChanged(); } } }
        public string FileType { get => fileType; set { if (fileType != value) { NotifyPropertyChanging(); fileType = value; NotifyPropertyChanged(); } } }
        public string LastModified { get => lastModified; set { if (lastModified != value) { NotifyPropertyChanging(); lastModified = value; NotifyPropertyChanged(); } } }
        public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }
        public int Extent { get => extent; set { if (extent != value) { NotifyPropertyChanging(); extent = value; NotifyPropertyChanged(); } } }
        public string MimeType { get => mimeType; set { if (mimeType != value) { NotifyPropertyChanging(); mimeType = value; NotifyPropertyChanged(); } } }

        public int GutenbergStyleIndexNumber
        {
            get
            {
                var id = BookId;
                var idx = id.IndexOf('/');
                if (idx < 0) return 0;
                var nstr = id.Substring(idx + 1);
                int gutIndex = 0;
                Int32.TryParse(nstr, out gutIndex);
                return gutIndex;
            }
        }


        public string FileTypeAsString()
        {
            switch (GetFileType())
            {
                case ProcessedFileType.CoverMedium: return "Image file (book cover)";
                case ProcessedFileType.CoverSmall: return "Image file (book cover)";
                case ProcessedFileType.EPub: return "EPUB";
                case ProcessedFileType.EPubNoImages: return "EPUB (no images)";
                case ProcessedFileType.Html: return "HTML web file";
                case ProcessedFileType.MobiPocket: return "Kindle (MobiPocket)";
                case ProcessedFileType.PDF: return "PDF";
                case ProcessedFileType.PS: return "PostScript";
                case ProcessedFileType.RDF: return "RDF Index File";
                case ProcessedFileType.Tex: return "Tex pre-press file";
                case ProcessedFileType.Text: return "Plain text file";
                case ProcessedFileType.TextNotUtf8: return "Plain text file";
                case ProcessedFileType.Unknown:
                default:
                    return $"Other file type ({MimeType})";
            }
        }

        public ProcessedFileType GetFileType()
        {
            switch (MimeType)
            {
                case "application/epub+zip":
                    //FAIL: they have two different patterns for epubs with images.
                    return (FileName.Contains(".images") || FileName.Contains("-images.epub")) ? ProcessedFileType.EPub : ProcessedFileType.EPubNoImages;

                case "application/octet-stream": // seemingly obsolete -- used for old books only?
                    return ProcessedFileType.Unknown;

                case "application/pdf": // PDF file
                    return ProcessedFileType.PDF;

                case "application/postscript": // postscript, of course
                    return ProcessedFileType.PS;

                case "application/prs.tex": // TEX files!
                    return ProcessedFileType.Tex;

                case "application/rdf+xml": // the RDF file
                    return ProcessedFileType.RDF;

                case "application/x-mobipocket-ebook": // kindle
                    return ProcessedFileType.MobiPocket;

                case "application/zip": // HTML has two formats: /zip and /html
                    return ProcessedFileType.Html;

                case "image/jpeg": // cover images
                    if (String.IsNullOrEmpty(FileName)) return ProcessedFileType.CoverSmall;
                    if (FileName.Contains("cover.small")) return ProcessedFileType.CoverSmall;
                    if (FileName.Contains("cover.medium")) return ProcessedFileType.CoverMedium;
                    return ProcessedFileType.CoverSmall;

                case "text/html":
                case "text/html; charset=iso-8859-1":
                case "text/html; charset=us-ascii":
                    return ProcessedFileType.HtmlNotUtf8;

                case "text/html; charset=utf-8":
                    return ProcessedFileType.Html;

                case "text/plain":
                case "text/plain; charset=iso-8859-1":
                case "text/plain; charset=us-ascii":
                    return ProcessedFileType.TextNotUtf8;

                case "text/plain; charset=utf-8":
                    return ProcessedFileType.Text;
                default:
                    return ProcessedFileType.Unknown;
            }
        }

        public bool IsKnownMimeType
        {
            get
            {
                switch (MimeType)
                {
                    case "application/epub+zip":
                    case "application/msword": // word doc e.g. 10681 and 80+ others
                    case "application/octet-stream": // seemingly obsolete -- used for old books only?
                    case "application/pdf": // PDF file
                    case "application/postscript": // postscript, of course
                    case "application/prs.tei": // XML text file (about 520) -- see https://en.wikipedia.org/wiki/Text_Encoding_Initiative
                    case "application/prs.tex": // TEX files!
                    case "application/rdf+xml": // the RDF file
                    case "application/x-mobipocket-ebook": // kindle
                    case "application/x-iso9660-image": // USed by the CD and DVD projects e.g. 10802 -- about 200+
                    case "application/zip": // HTML has two formats: /zip and /html
                    case "audio/midi": // MIDI music files e.g. jingle bells 10535 about 2500+
                    case "audio/mp4": // MP4 e.g. 19450 about 9000+
                    case "audio/mpeg": // MPEG about 23000+
                    case "audio/ogg": // OGG VORBIS format about 23000+
                    case "audio/x-ms-wma": // Microsoft format e.g. 36567 (really, just that one)
                    case "audio/x-wav": //
                    case "image/gif": // cover images
                    case "image/jpeg": // cover images
                    case "image/png": // image
                    case "image/tiff": // image
                    case "text/html":
                    case "text/html; charset=iso-8859-1":
                    case "text/html; charset=iso-8859-2":
                    case "text/html; charset=iso-8859-15":
                    case "text/html; charset=us-ascii":
                    case "text/html; charset=utf-8":
                    case "text/html; charset=windows-1251":
                    case "text/html; charset=windows-1252":
                    case "text/html; charset=windows-1253":
                    case "text/plain":
                    case "text/plain; charset=big5": // just one, 11002
                    case "text/plain; charset=ibm850": // just one, 11522
                    case "text/plain; charset=iso-8859-1":
                    case "text/plain; charset=iso-8859-2": // about 13
                    case "text/plain; charset=iso-8859-3": // about 4
                    case "text/plain; charset=iso-8859-7": // about 5
                    case "text/plain; charset=iso-8859-15": // about 16
                    case "text/plain; charset=us-ascii":
                    case "text/plain; charset=utf-7": // about 2 both of them  7467
                    case "text/plain; charset=utf-8":
                    case "text/plain; charset=utf-16": // seriously? 1, 13083
                    case "text/plain; charset=windows-1250": // 
                    case "text/plain; charset=windows-1251": // 
                    case "text/plain; charset=windows-1252": // 
                    case "text/plain; charset=windows-1253": // 
                    case "text/rtf": // 
                    case "text/rtf; charset=iso-8859-1": // 
                    case "text/rtf; charset=us-ascii": // 
                    case "text/x-rst": // reStructured Text https://en.wikipedia.org/wiki/ReStructuredText
                    case "text/rst; charset=us-ascii": // reStructured Text https://en.wikipedia.org/wiki/ReStructuredText
                    case "text/xml":
                    case "text/xml; charset=iso-8859-1":
                    case "video/mpeg":
                    case "video/quicktime":
                    case "video/x-msvideo":
                        return true;
                    default:
                        return false;
                }
            }

        }

        public override string ToString()
        {
            return this.FileName;
        }
    }
}
