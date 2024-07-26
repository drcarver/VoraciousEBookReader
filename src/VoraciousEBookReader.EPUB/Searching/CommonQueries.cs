using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VoraciousEBookReader.EPUB.Enum;
using VoraciousEBookReader.EPUB.Interfaces;
using VoraciousEBookReader.EPUB.Models;

namespace VoraciousEBookReader.EPUB.Searching
{
    /// <summary>
    /// Contains all of the common queries in the book databases
    /// </summary>
    static class CommonQueries
    {
        private static int _NQueries = 0;
        public static int NQueries
        {
            get { return _NQueries; }
            set
            {
                _NQueries = value;
                if (_NQueries % 1000 == 0 && _NQueries > 0)
                {
                    ; // Spot to break on; we shouldn't have so many queries during normal operation.
                    // It's a database; the goal is 1 big query, not a bazillion small ones.
                }
            }
        }


        /// <summary>
        /// Adds the Book into the Book database, but only if it's not already present.
        /// If it's already 
        /// </summary>
        /// <param name="Books"></param>
        /// <returns>0=not added, 1=added. Technical is the count of the number added.</returns>

        public static int BookAdd(BooksContext bookdb, Book book, ExistHandling handling)
        {
            int retval = 0;
            NQueries++;
            lock (bookdb)
            {
                switch (handling)
                {
                    case ExistHandling.IfNotExists:
                        if (bookdb.Books.Find(book.BookId) == null)
                        {
                            bookdb.Books.Add(book);
                            retval++;
                        }
                        break;
                    case ExistHandling.CatalogOverrideFast:
                        {
                            var dbbook = bookdb.Books.Find(book.BookId);
                            if (dbbook == null)
                            {
                                bookdb.Books.Add(book);
                                retval++;
                            }
                            else // have to be smart.
                            {
                                if (dbbook.BookSource.StartsWith(dbbook.BookSourceBookMarkFile))
                                {
                                    // The database was added to from a bookmark file.
                                    // For these books, the dbbook top-level data isn't correct but the user data is correct.
                                    // At the same time, the new book top-level data IS correct, but the user data is not correct.
                                    dbbook.Merge(dbbook, book);
                                    retval++;
                                }
                            }
                        }
                        break;
                    case ExistHandling.SmartCatalogOverride:
                        {
                            var dbbook = bookdb.Books.Find(book.BookId);
                            if (dbbook == null)
                            {
                                bookdb.Books.Add(book);
                                retval++;
                            }
                            else // have to be smart.
                            {
                                if (dbbook.BookSource.StartsWith(dbbook.BookSourceBookMarkFile))
                                {
                                    // The database was added to from a bookmark file.
                                    // For these books, the dbbook top-level data isn't correct but the user data is correct.
                                    // At the same time, the new book top-level data IS correct, but the user data is not correct.
                                    dbbook.Merge(dbbook, book);
                                    retval++;
                                }
                                else
                                {
                                    // Grab the full data including the number of files
                                    dbbook = BookGetFiles(bookdb, book.BookId);
                                    var mustReplace = !dbbook.FilesMatchEpub(book, dbbook);
                                    if (mustReplace)
                                    {
                                        //FAIL: project Gutenberg LOVES changing their URLs. If the old list doesn't match the 
                                        // new list in number of files, then dump ALL the old values and replace them with the
                                        // new ones.
                                        // TODO: actually verify that the files match?
                                        // Can't use clear because it doesn't work: dbbook.Files.Clear();
                                        // (Seriously: it doesn't work because Files doesn't implement it and will throw)
                                        for (int i = dbbook.Files.Count - 1; i >= 0; i--)
                                        {
                                            dbbook.Files.RemoveAt(i);
                                        }
                                        foreach (var file in dbbook.Files)
                                        {
                                            if (file.Id != 0) file.Id = 0; // if it's straight from the catalog, it should have no id 
                                            dbbook.Files.Add(file);
                                        }
                                        retval++;
                                    }
                                }
                            }
                        }
                        break;
                }
                return retval;
            }
        }


        public static int BookCount(BooksContext bookdb)
        {
            NQueries++;
            lock (bookdb)
            {
                var retval = bookdb.Books.Count();
                return retval;
            }
        }

        public static Book BookGet(BooksContext bookdb, string bookId)
        {
            NQueries++;
            lock (bookdb)
            {
                var booklist = bookdb.Books
                .Where(b => b.BookId == bookId)
                .Include(b => b.People)
                .Include(b => b.Files)
                .Include(b => b.Review)
                .Include(b => b.Notes)
                .Include(b => b.Notes.Notes)
                .Include(b => b.DownloadData)
                .Include(b => b.NavigationData)
                .AsQueryable();
                ;
                var book = booklist.Where(b => b.BookId == bookId).FirstOrDefault();
                if (book != null && book.BookId == "ebooks/57")
                {
                    ; // A good place to hang a debugger on.
                }
                return book;
            }
        }

        /// <summary>
        /// Returns an abbreviated set of data with just the Files. This is used when merging
        /// a new catalog with an old catalog; the new catalog might have more files than the
        /// old catalog. This is super-common with the latest books which might just be available
        /// as .TXT files at the start.
        /// </summary>
        /// <param name="bookdb"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        public static Book BookGetFiles(BooksContext bookdb, string bookId)
        {
            NQueries++;
            lock (bookdb)
            {
                var booklist = bookdb.Books
                .Where(b => b.BookId == bookId)
                .Include(b => b.Files)
                .AsQueryable();
                ;
                var book = booklist.Where(b => b.BookId == bookId).FirstOrDefault();
                if (book != null && book.BookId.Contains("62548"))
                {
                    ; // A good place to hang a debugger on.
                }
                return book;
            }
        }

        public static List<Book> BookGetAllWhichHaveUserData(BooksContext bookdb)
        {
            NQueries++;
            lock (bookdb)
            {
                var booklist = bookdb.Books
                .Include(b => b.Review)
                .Include(b => b.Notes)
                .Include(b => b.Notes.Notes)
                .Include(b => b.DownloadData)
                .Include(b => b.NavigationData)
                .Where(b => b.Review != null || b.Notes != null || b.NavigationData != null)
                .ToList();
                ;
                return booklist;
            }
        }

        public static TimeSpan LengthForRecentChanges()
        {
            var recentTimeSpan = new TimeSpan(45, 0, 0, 0); // 45 days
            //var recentTimeSpan = new TimeSpan(0, 1, 0, 0); // For debugging: a paltry 1 hour -- used for debugging
            return recentTimeSpan;
        }
        public static List<Book> BookGetRecentWhichHaveUserData(BooksContext bookdb)
        {
            NQueries++;
            var now = DateTimeOffset.Now;
            var recentTimeSpan = LengthForRecentChanges();
            lock (bookdb)
            {
                var booklist = bookdb.Books
                .Include(b => b.Review)
                .Include(b => b.Notes)
                .Include(b => b.Notes.Notes)
                .Include(b => b.DownloadData)
                .Include(b => b.NavigationData)
                .Where(b => b.Review != null || b.Notes != null || b.NavigationData != null)
                .ToList()
                .Where(b => now.Subtract(b.NavigationData.MostRecentNavigationDate) < recentTimeSpan)
                .ToList()
                ;
                return booklist;
            }
        }
        public static Task FirstSearchToWarmUpDatabase()
        {
            Task mytask = Task.Run(() =>
            {
                NQueries++;
                //var bookdb = BooksContext.Get();
                //lock (bookdb)
                {
                    DoCreateIndexFile();
                }
            });
            return mytask;
        }

        public static void BookDoMigrate(BooksContext bookdb)
        {
            NQueries++;
            bookdb.DoMigration();
        }
        public static void BookRemoveAll(BooksContext bookdb)
        {
            NQueries++;
            lock (bookdb)
            {
                foreach (var book in bookdb.Books)
                {
                    bookdb.Books.Remove(book);
                }
            }
        }

        public static void BookSaveChanges(BooksContext bookdb)
        {
            NQueries++;
            lock (bookdb)
            {
                bookdb.SaveChanges();
            }
        }

        public static int BookNavigationDataAdd(BooksContext bookdb, Navigation bn, ExistHandling handling)
        {
            int retval = 0;
            NQueries++;
            var book = BookGet(bookdb, bn.BookId);
            if (book == null) return retval;
            switch (handling)
            {
                case ExistHandling.IfNotExists:
                    if (book.Navigation == null)
                    {
                        book.Navigation = bn;
                        retval++;
                    }
                    break;
            }
            return retval;
        }


        public static Navigation BookNavigationDataEnsure(BooksContext bookdb, Book Books)
        {
            var nd = BookNavigationDataFind(bookdb, Books.BookId);
            if (nd == null)
            {
                nd = new Navigation()
                {
                    BookId = Books.BookId,
                };
                CommonQueries.BookNavigationDataAdd(bookdb, nd, ExistHandling.IfNotExists);
                nd = BookNavigationDataFind(bookdb, Books.BookId);
                BookSaveChanges(bookdb);
            }
            if (nd == null)
            {
                App.Error($"ERROR: trying to ensure navigation data, but don't have one.");
            }
            return nd;
        }



        public static Navigation BookNavigationDataFind(BooksContext bookdb, string bookId)
        {
            NQueries++;
            var book = BookGet(bookdb, bookId);
            if (book == null)
            {
                App.Error($"ERROR: attempting to get navigation data for a book={bookId} that doesn't exist");
                return null;
            }
            var retval = book.Navigation;
            return retval;
        }



        public static int BookNotesAdd(BooksContext bookdb, BookNotes bn, ExistHandling handling)
        {
            int retval = 0;
            NQueries++;
            var book = BookGet(bookdb, bn.BookId);
            if (book == null) return retval;
            switch (handling)
            {
                case ExistHandling.IfNotExists:
                    if (book.Notes == null)
                    {
                        book.Notes = bn;
                        retval++;
                    }
                    break;
            }
            return retval;
        }

        public static BookNotes BookNotesFind(BooksContext bookdb, string bookId)
        {
            NQueries++;
            var book = BookGet(bookdb, bookId);
            var retval = book.Notes;
            return retval;
        }

        public static void BookNoteSave(BooksContext bookdb, UserNote note)
        {
            var bn = BookNotesFind(bookdb, note.BookId);
            if (bn == null)
            {
                bn = new BookNotes();
                bn.BookId = note.BookId;
                CommonQueries.BookNotesAdd(bookdb, bn, ExistHandling.IfNotExists);
                bn = BookNotesFind(bookdb, note.BookId);
            }
            if (note.Id == 0) // Hasn't been saved before. The id is 0.
            {
                bn.Notes.Add(note);
            }
            BookSaveChanges(bookdb);
        }

        public static IList<BookNotes> BookNotesGetAll()
        {
            NQueries++;
            var bookdb = BooksContext.Get();

            var retval = bookdb.Books
                .Include(b => b.Notes)
                .Where(b => b.Notes != null)
                .Include(b => b.Notes.Notes)
                .Select(b => b.Notes)
                .ToList();
            return retval;
        }

        public static int DownloadedBookAdd(BooksContext bookdb, Download dd, ExistHandling handling)
        {
            int retval = 0;
            NQueries++;
            var book = BookGet(bookdb, dd.BookId);
            if (book == null) return retval;
            switch (handling)
            {
                case ExistHandling.IfNotExists:
                    if (book.Download == null)
                    {
                        book.Download = dd;
                        retval++;
                    }
                    break;
            }
            return retval;
        }


        public static void DownloadedBookEnsureFileMarkedAsDownloaded(BooksContext bookdb, string bookId, string folderPath, string filename)
        {
            NQueries++;
            var book = BookGet(bookdb, bookId);
            if (book == null)
            {
                App.Error($"ERROR: trying to ensure that {bookId} is downloaded, but it's not a valid book");
                return;
            }
            var dd = book.Download;
            if (dd == null)
            {
                dd = new Download()
                {
                    BookId = bookId,
                    FilePath = folderPath,
                    FileName = filename,
                    CurrFileStatus = FileStatus.Downloaded,
                    DownloadDate = DateTimeOffset.Now,
                };
                book.DenormDownloadDate = dd.DownloadDate.ToUnixTimeSeconds();
                CommonQueries.DownloadedBookAdd(bookdb, dd, ExistHandling.IfNotExists);
                BookSaveChanges(bookdb);
            }
            else if (dd.CurrFileStatus != FileStatus.Downloaded)
            {
                dd.FilePath = folderPath;
                dd.CurrFileStatus = FileStatus.Downloaded;
                BookSaveChanges(bookdb);
            }
        }

        public static Download DownloadedBookFind(BooksContext bookdb, string bookId)
        {
            NQueries++;
            var book = BookGet(bookdb, bookId);
            if (book == null)
            {
                App.Error($"ERROR: attempting to get download data for {bookId} that isn't in the database");
                return null;
            }
            var retval = book.Download;
            return retval;
        }

        public static List<Download> DownloadedBooksGetAll(BooksContext bookdb)
        {
            NQueries++;
            lock (bookdb)
            {
                var bookquery = from b in bookdb.Books where b.DownloadData != null select b.DownloadData;
                var retval = bookquery.ToList();
                return retval;
            }
        }


        public static int UserReviewAdd(BooksContext bookdb, UserReview review, ExistHandling handling)
        {
            int retval = 0;
            NQueries++;
            var book = BookGet(bookdb, review.BookId);
            if (book == null) return retval;
            switch (handling)
            {
                case ExistHandling.IfNotExists:
                    if (book.UserReview == null)
                    {
                        book.UserReview = review;
                        retval++;
                    }
                    break;
            }
            return retval;
        }

        public static UserReview UserReviewFind(BooksContext bookdb, string bookId)
        {
            NQueries++;
            var book = BookGet(bookdb, bookId);
            if (book == null)
            {
                App.Error($"ERROR: attempting to get user review for {bookId} that isn't in the database");
                return null;
            }
            var retval = book.UserReview;
            return retval;
        }

        public static List<UserReview> UserReviewsGetAll(BooksContext bookdb)
        {
            NQueries++;
            lock (bookdb)
            {
                var bookquery = from b in bookdb.Books 
                                where b.Review != null 
                                select b.Review; // NOTE: update all queries to use the dotted format with includes
                var retval = bookquery.ToList();
                return retval;
            }
        }
        #region FAST_SEARCH
        class BookIndex
        {
            public string BookId { get; set; }
            public string Text { get; set; }
            public override string ToString()
            {
                return $"{BookId}\t{Text}"; // assumes bookId will never include a tab.
            }
            public static BookIndex FromBooks(Book Books)
            {
                var sb = new StringBuilder();
                Append(sb, Books.Title);
                Append(sb, Books.TitleAlternative);
                Append(sb, Books.UserReview?.Tags);
                Append(sb, Books.UserReview?.Review);
                Append(sb, Books.BookSeries);
                Append(sb, Books.Imprint);
                Append(sb, Books.LCC);
                Append(sb, Books.LCCN);
                Append(sb, Books.LCSH);
                if (Books.PGNotes != null)
                {
                    foreach (var note in Books.PGNotes.Notes)
                    {
                        Append(sb, note.Tags);
                        Append(sb, note.Text);
                    }
                }
                foreach (var people in Books.People)
                {
                    Append(sb, people.Aliases);
                    Append(sb, people.Name);
                }

                var retval = new BookIndex()
                {
                    BookId = Books.BookId,
                    Text = sb.ToString(),
                };
                return retval;
            }
            public static StringBuilder Append(StringBuilder sb, string field)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    sb.Append(" ");
                    bool sawWS = true;
                    foreach (var ch in field)
                    {
                        var newch = char.ToLower(ch);
                        if (newch < '0'
                            || newch >= ':' && newch <= '@'
                            || newch >= '[' && newch <= '`'
                            || newch >= '{' && newch <= '~'
                            ) // higher chars stay the same. International support is ... iffy ... 
                            newch = ' ';
                        if (newch != ' ' || !sawWS)
                        {
                            sb.Append(newch);
                        }
                        sawWS = newch == ' ';
                    }
                }
                return sb;
            }
        }
        static Dictionary<string, BookIndex> BookIndexes = null;

        public static void DoCreateIndexFile()
        {
            if (BookIndexes != null) return;
            BookIndexes = new Dictionary<string, BookIndex>();

            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var path = folder.Path;
            string dbpath = Path.Combine(path, BooksContext.BooksDatabaseFilename);

            var startTime = DateTime.UtcNow;
            using (var connection = new SqliteConnection($"Data Source={dbpath}"))
            {
                connection.Open();
                AddFromTable(connection, true, "SELECT BookId,Title,TitleAlternative,LCSH,LCCN,LCC,BookSeries FROM Book");
                AddFromTable(connection, false, "SELECT BooksBookId,Name,Aliases FROM Person");
                AddFromTable(connection, false, "SELECT BookId,Text,Tags FROM UserNote");
                AddFromTable(connection, false, "SELECT BookId,Review,Tags FROM UserReview");
            }
            var delta = DateTime.UtcNow.Subtract(startTime).TotalSeconds;
#if NEVER_EVER_DEFINED
            for (int i=0; i<30_000; i+=100)
            {
                // Artificially wait 30 seconds
                Task.Delay(100).Wait();
            }
#endif
            Logger.Log($"Time to read index: {delta} seconds");
            ;
        }

        private static void AddFromTable(SqliteConnection connection, bool create, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var bookId = reader.GetString(0);
                    var sb = new StringBuilder();
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            BookIndex.Append(sb, reader.GetString(i));
                        }
                    }
                    if (create)
                    {
                        var index = new BookIndex() { BookId = bookId, Text = sb.ToString() };
                        BookIndexes.Add(index.BookId, index);
                    }
                    else
                    {
                        try
                        {
                            var index = BookIndexes[bookId];
                            index.Text += sb.ToString();
                        }
                        catch (Exception)
                        {
                            ; // Error; why doesn't the book exist?
                        }
                    }
                }
            }
        }
        public static void DoCreateIndexFileEF()
        {
            if (BookIndexes != null) return;
            BookIndexes = new Dictionary<string, BookIndex>();

            var bookdb = BooksContext.Get();
            var bookList = bookdb.Books
             .Include(b => b.People)
             .Include(b => b.Review)
             .Include(b => b.Notes)
             .Include(b => b.Notes.Notes)
             .ToList();
            //var sb = new StringBuilder();
            foreach (var Books in bookList)
            {
                var index = BookIndex.FromBooks(Books);
                BookIndexes.Add(index.BookId, index);
                //sb.Append(index.ToString());
                //sb.Append('\n');
            }
            //var fullIndex = sb.ToString();
            ;
        }
        public static HashSet<string> BookSearchs(ISearch searchOperations)
        {
            DoCreateIndexFile(); // create the index as needed.
            var retval = new HashSet<string>();
            foreach (var (bookid, index) in BookIndexes)
            {
                var hasSearch = searchOperations.MatchesFlat(index.Text);
                if (hasSearch)
                {
                    retval.Add(index.BookId);
                }
            }
            return retval;
        }

        #endregion

    }
}
