//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;

//using CommunityToolkit.Mvvm.ComponentModel;

//using VoraciousEBookReader.EPUB.Enum;
//using VoraciousEBookReader.EPUB.Interfaces;
//using VoraciousEBookReader.EPUB.Models;

//namespace VoraciousEBookReader.EPUB.ViewModels
//{
//    /// <summary>
//    /// One Gutenberg record for a book (not all data is saved)
//    /// </summary>
//    public partial class BookViewModel : ObservableObject, IGetSearchArea
//    {
//        /// <summary>
//        /// People include authors, illustrators, etc.
//        /// </summary>
//        [ObservableProperty]
//        private ObservableCollection<Person> people = new ObservableCollection<Person>();

//        public string BestAuthorDefaultIsNull
//        {
//            //get
//            //{
//            //    var personlist = from person in People orderby person.GetImportance() ascending select person;
//            //    var author = personlist.FirstOrDefault();
//            //    if (author == null)
//            //    {
//            //        return null;
//            //    }
//            //    return author.Name;
//            //}
//        }

//        /// <summary>
//        /// Get a shortened title with author name suitable for being a filename.
//        /// </summary>
//        /// <returns></returns>
//        public string GetBestTitleForFilename()
//        {
//            var personlist = from person in People orderby person.GetImportance() ascending select person;
//            var author = personlist.FirstOrDefault();
//            return TitleConverter(Title, author?.Name);
//        }

//        const int NICE_MIN_LEN = 20;
//        const int NICE_MAX_LEN = 30;

//        [ObservableProperty]
//        private string bookId;

//        [ObservableProperty]
//        private string bookSource = BookSourceGutenberg;

//        [ObservableProperty]
//        private FileType bookType = FileType.other;

//        /// <summary>
//        /// Examples:
//        /// <dcterms:description>There is an improved edition of this title, eBook #29888</dcterms:description>
//        /// <dcterms:description>Illustrated by the author.</dcterms:description>
//        /// </summary>
//        [ObservableProperty]
//        private string description;

//        /// <summary>
//        /// Examples:
//        /// #28: <pgterms:marc260>Houston: Advantage International, The PaperLess Readers Club, 1992</pgterms:marc260>
//        /// </summary>
//        [ObservableProperty]
//        private string imprint;

//        [ObservableProperty]
//        private string issued = string.Empty;

//        /// <summary>
//        /// <dcterms:title>Three Little Kittens</dcterms:title>
//        /// </summary>
//        [ObservableProperty]
//        private string title;

//        /// <summary>
//        /// Used when there is already a title
//        /// <dcterms:alternative>Alice in Wonderland</dcterms:alternative>
//        /// </summary>
//        [ObservableProperty]
//        private string titleAlternative;

//        /// <summary>
//        /// List of all of the files for this book and their formats.
//        /// </summary>
//        [ObservableProperty]
//        private ObservableCollection<FilenameAndFormat> files = new ObservableCollection<FilenameAndFormat>();

//        [ObservableProperty]
//        private string language;

//        // e.g. en. Apress raw data can be capitalized as En, which IMHO is wrong.
//        /// <summary>
//        /// <dcterms:subject>
//        ///     <rdf:Description rdf:nodeID="N0d26c4c9a07a454789d1f6545628914b">
//        ///         <rdf:value>Cats -- Juvenile fiction</rdf:value>
//        ///         <dcam:memberOf rdf:resource= "http://purl.org/dc/terms/LCSH" />
//        ///     </rdf:Description>
//        ///     </dcterms:subject>
//        /// </summary>
//        [ObservableProperty]
//        private string lCSH = string.Empty;

//        [ObservableProperty]
//        private string lCCN = string.Empty;

//        // Marc010 e.g. 18020634 is https://catalog.loc.gov/vwebv/search?searchArg=18020634&searchCode=GKEY%5E*&searchType=0&recCount=25&sk=en_US
//        [ObservableProperty]
//        private string pGEditionInfo;

//        // Marc250
//        [ObservableProperty]
//        private string pGProducedBy;

//        // Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),\n
//        [ObservableProperty]
//        private string pGNotes;

//        // Marc546 e.g. This ebook uses a 19th century spelling for pg11299.rdf
//        [ObservableProperty]
//        private string bookSeries;

//        /// <summary>
//        /// Example. Note that 'subject' might be LCC or LCSH
//        /// <dcterms:subject>
//        ///     <rdf:Description rdf:nodeID="N5e552155376c46acba0f56226354c4a8">
//        ///         <dcam:memberOf rdf:resource="http://purl.org/dc/terms/LCC"/>
//        ///         <rdf:value>PZ</rdf:value>
//        ///     </rdf:Description>
//        /// </dcterms:subject>
//        /// Marc440 e.g. The Pony Rider Boys, number 8 for pg12980.rdf
//        /// </summary>
//        [ObservableProperty]
//        private string lCC = "";

//        //
//        // De-normalized data used the make sorting go faster
//        //
//        // is the PZ. Is a CSV because e.g. book 1 is both JK and E201
//        [ObservableProperty]
//        private string denormPrimaryAuthor;

//        // UNIX time in seconds
//        [ObservableProperty]
//        private long denormDownloadDate; 

//        //
//        // Next is all of the user-settable things
//        //
//        [ObservableProperty]
//        private UserReview review;

//        [ObservableProperty]
//        private BookNotes notes;
        
//        [ObservableProperty]
//        private Download download;
        
//        [ObservableProperty]
//        private Navigation navigation;

//        /// <summary>
//        /// Used by the TitleConverter to get a nice potential filename from the title+author.
//        /// The file should include both title and author if possible
//        /// </summary>
//        /// <param name="value"></param>
//        /// <param name="min"></param>
//        /// <param name="max"></param>
//        /// <returns></returns>
//        private static string ChopString(string value, int min = NICE_MIN_LEN, int max = NICE_MAX_LEN)
//        {
//            if (value == null) return value;
//            if (value.Length > min)
//            {
//                var nextspace = value.IndexOf(' ', min);
//                if (nextspace < 0 && value.Length < max)
//                {
//                    ; // no next space, but the total title isn't too long. Keep it as-is
//                }
//                else if (nextspace < 0)
//                {
//                    // Too long, and no next space at all. Chop it ruthlessly.
//                    value = value.Substring(0, min);
//                }
//                else if (nextspace < max)
//                {
//                    value = value.Substring(0, nextspace);
//                }
//                else
//                {
//                    // Too long, and no convenient next space. Chop it ruthlessly.
//                    value = value.Substring(0, min);
//                }
//            }
//            value = value.Trim();
//            return value;
//        }

//        /// <summary>
//        /// Given a title and author, generate a nice possible file string. Uses ASCII 
//        /// only (sorry, everyone with a name or title that doesn't convert)
//        /// </summary>
//        /// <param name="title"></param>
//        /// <param name="author"></param>
//        /// <returns></returns>
//        private static string TitleConverter(string title, string author)
//        {
//            title = ChopString(title);
//            author = ChopString(author);
//            var potentialRetval = author == null ? title : $"{title}_by_{author}";
//            potentialRetval = potentialRetval.Replace(" , ", ",").Replace(", ", ","); // because smith , jane is better as smith_jane
//            char[] remove = { '\'', '.' };
//            var sb = new StringBuilder();
//            foreach (var ch in potentialRetval)
//            {
//                char c = ch;
//                if (!remove.Contains(c))
//                {
//                    if (char.IsControl(ch)) c = '-';
//                    else if (char.IsWhiteSpace(ch)) c = '_';
//                    else if (char.IsLetterOrDigit(ch)) c = ch;
//                    else if (ch < 128) c = '_';
//                    else c = ch; // allow all of the unicode chars
//                    sb.Append(c);
//                }
//            }
//            var retval = sb.ToString();
//            retval = retval.Trim('_');
//            return retval;
//        }

//        /// <summary>
//        /// Return either "" (is valid) or a loggable string of why the book has problems.
//        /// </summary>
//        /// <returns></returns>
//        public string Validate()
//        {
//            var retval = "";
//            if (string.IsNullOrWhiteSpace(BookId)) retval += "ERROR: BookId is not set\n";
//            if (string.IsNullOrWhiteSpace(Title)) retval += "ERROR: Title is not set\n";
//            if (!string.IsNullOrWhiteSpace(TitleAlternative) && string.IsNullOrWhiteSpace(Title)) retval += "ERROR: TitleAlternative is set but Title is not\n";

//            if (Issued == "None") retval += "ERROR: Book was not issued\n";
//            if (Title == "No title" && Files.Count == 0) retval += "ERROR: Gutenberg made a book with no title or files";
//            if (retval != "" && Files.Count == 0) retval += "ERROR: Book has no files";
//            return retval;
//        }

//        /// <summary>
//        /// <dcterms:language>
//        ///     <rdf:Description rdf:nodeID="Nc3827dd334c44413ab159b8f40d432ec">
//        ///         <rdf:value rdf:datatype="http://purl.org/dc/terms/RFC4646">en</rdf:value>
//        ///     </rdf:Description>
//        /// </dcterms:language>
//        /// </summary>
//        /// 
//        public static bool FilesMatch(BookViewModel a, BookViewModel b)
//        {
//            var retval = true;
//            foreach (var afile in a.Files)
//            {
//                if (FileIsKindle(afile.FileName)) continue; // Don't care about kindle files 
//                var hasMatch = false;
//                foreach (var bfile in b.Files)
//                {
//                    if (afile.FileName == bfile.FileName)
//                    {
//                        hasMatch = true;
//                        break;
//                    }
//                }
//                if (!hasMatch)
//                {
//                    retval = false;
//                    break;
//                }
//            }
//            return retval;
//        }

//        public static bool FilesMatchEpub(BookViewModel a, BookViewModel b)
//        {
//            var retval = true;
//            foreach (var afile in a.Files)
//            {
//                if (!FileIsEpub(afile.FileName)) continue; // Don't care about non-epub 
//                var hasMatch = false;
//                foreach (var bfile in b.Files)
//                {
//                    if (afile.FileName == bfile.FileName)
//                    {
//                        hasMatch = true;
//                        break;
//                    }
//                }
//                if (!hasMatch)
//                {
//                    retval = false;
//                    break;
//                }
//            }
//            return retval;
//        }

//        public static bool FileIsEpub(string fname)
//        {
//            // why does Gutenberg do this :-(
//            var retval = fname.EndsWith(".epub") 
//                || fname.Contains(".epub."); 
//            return retval;
//        }

//        public static bool FileIsKindle(string fname)
//        {
//            var retval = fname.Contains(".kindle.");
//            return retval;
//        }

//        public static bool FilesIncludesEpub(BookViewModel bd)
//        {
//            var retval = false;
//            foreach (var afile in bd.Files)
//            {
//                if (FileIsEpub(afile.FileName))
//                {
//                    retval = true;
//                    break; // Once I find one, that is good enough.
//                }
//            }
//            return retval;
//        }

//        // Used by the search system
//        public IList<string> GetSearchArea(string inputArea)
//        {
//            var retval = new List<string>();
//            var area = (inputArea + "...").Substring(0, 3).ToLower(); // e.g. title --> ti
//            switch (area)
//            {
//                case "...":
//                    AddTitle(retval);
//                    AddPeople(retval);
//                    AddLCC(retval);
//                    AddNotes(retval);
//                    break;

//                case "tit": // title
//                    AddTitle(retval);
//                    break;

//                case "by.":
//                    AddPeople(retval);
//                    break;

//                case "aut": // author is part of by
//                    foreach (var person in People)
//                    {
//                        if (person.PersonType == Relator.author
//                            || person.PersonType == Relator.artist
//                            || person.PersonType == Relator.collaborator
//                            || person.PersonType == Relator.contributor
//                            || person.PersonType == Relator.dubiousAuthor)
//                        {
//                            retval.Add(person.Name);
//                            if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
//                        }
//                    }
//                    break;

//                case "edi": // editor is part of by
//                    foreach (var person in People)
//                    {
//                        if (person.PersonType == Relator.editor
//                            || person.PersonType == Relator.editorOfCompilation
//                            || person.PersonType == Relator.printer
//                            || person.PersonType == Relator.publisher
//                            )
//                        {
//                            retval.Add(person.Name);
//                            if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
//                        }
//                    }
//                    break;

//                case "lc.": // e.g. just the LCC=PS or the LCN=E305
//                    if (!string.IsNullOrEmpty(LCC)) retval.Add(LCC);
//                    if (!string.IsNullOrEmpty(LCCN)) retval.Add(LCCN);
//                    break;
//                case "lcc": // LCC includes all LC
//                    AddLCC(retval);
//                    break;

//                case "ill": // illustrator is part of by
//                    foreach (var person in People)
//                    {
//                        if (person.PersonType == Relator.illustrator
//                            || person.PersonType == Relator.artist
//                            || person.PersonType == Relator.engraver
//                            || person.PersonType == Relator.photographer)
//                        {
//                            retval.Add(person.Name);
//                            if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
//                        }
//                    }
//                    break;

//                case "not": // notes and reviews
//                    AddNotes(retval);
//                    break;

//                case "ser": // series, like the Pony Rider (is part of title)
//                    if (!string.IsNullOrEmpty(BookSeries)) retval.Add(BookSeries);
//                    break;

//                case "sub": // lcc subject headings e.g. just the LCC=PS or the LCN=E305
//                    if (!string.IsNullOrEmpty(LCSH)) retval.Add(LCSH);
//                    break;
//            }
//            return retval;
//        }

//        private void AddLCC(List<string> retval)
//        {
//            if (!string.IsNullOrEmpty(LCC)) retval.Add(LCC);
//            if (!string.IsNullOrEmpty(LCCN)) retval.Add(LCCN);
//            if (!string.IsNullOrEmpty(LCSH)) retval.Add(LCSH);
//        }

//        private void AddNotes(List<string> retval)
//        {
//            if (!string.IsNullOrEmpty(Review?.Tags)) retval.Add(Review.Tags);
//            if (!string.IsNullOrEmpty(Review?.Review)) retval.Add(Review.Review);
//            if (Notes != null && Notes.Notes != null && Notes.Notes.Count > 0)
//            {
//                foreach (var note in Notes.Notes)
//                {
//                    if (!string.IsNullOrEmpty(note.Tags)) retval.Add(note.Tags);
//                    if (!string.IsNullOrEmpty(note.Text)) retval.Add(note.Text);
//                }
//            }
//        }

//        private void AddPeople(List<string> retval)
//        {
//            foreach (var person in People)
//            {
//                retval.Add(person.Name);
//                if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
//            }
//        }

//        private void AddTitle(List<string> retval)
//        {
//            retval.Add(Title);
//            if (!string.IsNullOrEmpty(TitleAlternative)) retval.Add(TitleAlternative);
//            if (!string.IsNullOrEmpty(BookSeries)) retval.Add(BookSeries);
//        }

//        /// <summary>
//        /// Merge two book data items together where one is directly from a catalog and has
//        /// no user data (like a review or notes)
//        /// </summary>
//        /// <param name="existing"></param>
//        /// <param name="catalog"></param>
//        public static void Merge(BookViewModel existing, BookViewModel catalog)
//        {
//            // book id: keep existing
//            existing.BookSource = catalog.BookSource;
//            existing.BookType = catalog.BookType;
//            existing.Description = catalog.Description;
//            existing.Imprint = catalog.Imprint;
//            existing.Issued = catalog.Issued;
//            existing.Title = catalog.Title;
//            existing.TitleAlternative = catalog.TitleAlternative;
//            while (existing.People.Count > 0)
//            {
//                existing.People.RemoveAt(0);
//            }
//            // To heck with collections that can't clear! existing.People.Clear();
//            foreach (var person in catalog.People)
//            {
//                if (person.Id != 0) person.Id = 0; // Straight from a catalog there should be no person id values set.
//                existing.People.Add(person);
//            }
//            while (existing.Files.Count > 0)
//            {
//                existing.Files.RemoveAt(0);
//            }
//            // to heck with...existing.Files.Clear();
//            foreach (var file in catalog.Files)
//            {
//                if (file.Id != 0) file.Id = 0; // Straight from a catalog there should be no file id values set.
//                existing.Files.Add(file);
//            }
//            existing.Language = catalog.Language;
//            existing.LCSH = catalog.LCSH;
//            existing.LCCN = catalog.LCCN;
//            existing.PGEditionInfo = catalog.PGEditionInfo;
//            existing.PGProducedBy = catalog.PGProducedBy;
//            existing.PGNotes = catalog.PGNotes;
//            existing.BookSeries = catalog.BookSeries;
//            existing.LCC = catalog.LCC;
//            existing.DenormPrimaryAuthor = catalog.DenormPrimaryAuthor;
//        }

//        public override string ToString()
//        {
//            return $"{Title.Substring(0, Math.Min(Title.Length, 20))} for {BookId}";
//        }
//    }
//}
