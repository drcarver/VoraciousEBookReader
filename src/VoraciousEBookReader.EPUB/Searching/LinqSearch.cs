using System.Collections.Generic;
using System.Linq;

using VoraciousEBookReader.EPUB.Enum;
using VoraciousEBookReader.EPUB.Interfaces;
using VoraciousEBookReader.EPUB.Models;

namespace VoraciousEBookReader.EPUB.Searching
{
    static public class AllBookSearch
    {
        public const int MaxMatch = 300;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookdb"></param>
        /// <param name="searchScope">One of Catalog, Pick, Reading, Downloaded, Finished, CopiedToEBookReader</param>
        /// <param name="search">search query like title:voyage sea</param>
        /// <param name="language">en or *</param>
        /// <param name="sortBy">One of author, title, date_download_asc, date_download_desc</param>
        /// <param name="andMore"></param>
        /// <returns></returns>
        //static public List<Book> SearchInternal(BooksContext bookdb, string searchScope, string search, string language, string sortBy, out bool andMore)
        //{
        //    // Query list is for the part of the query that can be done in the database.
        //    // This includes picking what data is returned, simple selection based on whether
        //    // a book is downloaded or not, and sorting.
        //    IQueryable<Book> queryList = bookdb.Books;

        //    // Enumerable list is after getting a superset of books from the database. It will 
        //    // reduce the number of books based on the actual search query.
        //    IEnumerable<Book> enumerableList = null;

        //    // Final list of books to return.
        //    var resultList = new List<Book>();

        //    //languages: cases are "*", "en" or anything else.
        //    // * get all language; "en" get books in English OR with no language, anything else requires that language.

        //    LinqIncludes includes = LinqIncludes.None;
        //    ISearch searchOperations = string.IsNullOrWhiteSpace(search)
        //        ? null
        //        : SearchParser.Parse(search);

        //    HashSet<string> idlist = searchOperations != null ? CommonQueries.BookSearchs(searchOperations) : null;
        //    const string DefaultLanguage = "en";

        //    bool mustIncludeEpub = false;

        //    if (language == "*")
        //    {
        //        ; // nothing special to restrict the languages
        //    }
        //    else if (language == DefaultLanguage) // most books that aren't marked are english. 
        //    {
        //        includes |= LinqIncludes.LanguageExactOrNull;
        //    }
        //    else
        //    {
        //        includes |= LinqIncludes.LanguageExact;
        //    }


        //    switch (searchScope)
        //    {
        //        default:
        //            App.Error($"Unknown search scope {searchScope}");
        //            includes |= LinqIncludes.UserData | LinqIncludes.People;
        //            includes &= ~LinqIncludes.LanguagesFlags;
        //            break;

        //        case "Catalog":
        //            includes |= LinqIncludes.UserData | LinqIncludes.People;
        //            includes &= ~LinqIncludes.LanguagesFlags;

        //            break;
        //        case "PickToDownload": // pick book to download. Have to be extra good with the query so it's fast.
        //            includes |= LinqIncludes.DownloadData | LinqIncludes.Files;
        //            if (!string.IsNullOrEmpty(search))
        //            {
        //                includes |= LinqIncludes.People;
        //            }
        //            break;
        //        case "Reading":
        //        case "Downloaded":
        //        case "Finished":
        //        case "CopiedToEBookReader":
        //            includes |= LinqIncludes.NavigationData | LinqIncludes.UserData | LinqIncludes.People; // includes download
        //            break;
        //    }

        //    if (idlist != null) queryList = queryList.Where(b => idlist.Contains(b.BookId));
        //    //switch (includes.HasFlag)
        //    //{
        //    //    case LinqIncludes.People:

        //    //}
        //    if (includes.HasFlag(LinqIncludes.People)) queryList = queryList.Include(b => b.People);
        //    if (includes.HasFlag(LinqIncludes.Notes)) queryList = queryList.Include(b => b.BookNotes).Include(b => b.BookNotes.Notes);
        //    if (includes.HasFlag(LinqIncludes.Review)) queryList = queryList.Include(b => b.Review);
        //    if (includes.HasFlag(LinqIncludes.DownloadData)) queryList = queryList.Include(b => b.DownloadData);
        //    if (includes.HasFlag(LinqIncludes.NavigationData)) queryList = queryList.Include(b => b.NavigationData);
        //    if (includes.HasFlag(LinqIncludes.LanguageExact)) queryList = queryList.Where(b => b.Language == language);
        //    if (includes.HasFlag(LinqIncludes.LanguageExactOrNull)) queryList = queryList.Where(b => string.IsNullOrEmpty(b.Language) || b.Language == language);

        //    //
        //    // The include list (query able) is set up correctly.
        //    // Now do the match list.
        //    //
        //    IQueryable<Book> matchList;

        //    switch (searchScope)
        //    {
        //        default:
        //        case "Catalog":
        //            matchList = queryList;
        //            break;

        //        case "PickToDownload":
        //            // TODO: doesn't work across computers? The DownloadData seems to always be NULL in the bookmarks
        //            // which means when I switch computers, all the books I've downloaded read and finished
        //            // will show up in the list?
        //            matchList = queryList
        //                .Where(b => b.Download == null || b.Download.CurrFileStatus != FileStatus.Downloaded)
        //                .Where(b => b.Navigation == null
        //                    || b.Navigation.NSwipeLeft < 1
        //                        && b.Navigation.CurrStatus == UserStatus.NoStatus
        //                    )
        //                ;
        //            mustIncludeEpub = true;
        //            break;
        //        case "Downloaded":
        //            matchList = queryList
        //                .Where(b => b.Download != null && b.Download.CurrFileStatus == FileStatus.Downloaded)
        //                .Where(b => b.Navigation == null
        //                    || b.Navigation.NSwipeLeft < 1
        //                        && b.Navigation.CurrStatus == UserStatus.NoStatus
        //                    )
        //                ;
        //            break;
        //        case "Reading":
        //            matchList = queryList
        //                .Where(b => b.Download != null && b.Download.CurrFileStatus == FileStatus.Downloaded)
        //                .Where(b => b.Navigation != null)
        //                .Where(b => b.Navigation.NSwipeLeft < 1)
        //                .Where(b => b.Navigation.CurrStatus == UserStatus.Reading)
        //                ;
        //            break;
        //        case "Finished":
        //            matchList = queryList
        //                .Where(b => b.Navigation != null)
        //                .Where(b => b.Navigation.NSwipeLeft < 1)
        //                .Where(b =>
        //                    b.Navigation.CurrStatus == UserStatus.Abandoned
        //                    || b.Navigation.CurrStatus == UserStatus.Done)
        //                ;
        //            // Match list used to also insist that the book be downloaded. But in reality, I might 
        //            // finish a book on computer "A" and then want to see that it's finished on computer "B"
        //            //.Where(b => b.DownloadData != null) 
        //            break;
        //        case "CopiedToEBookReader":
        //            matchList = queryList
        //                .Where(b => b.Navigation != null)
        //                .Where(b => b.Navigation.NSwipeLeft < 1)
        //                .Where(b => b.Navigation.CurrStatus == UserStatus.CopiedToEBookReader)
        //                ;
        //            break;
        //    }
        //    // Add in sorting
        //    switch (sortBy)
        //    {
        //        default:
        //        case "title":
        //            matchList = matchList.OrderBy(b => b.Title);
        //            break;
        //        case "author":
        //            matchList = matchList.OrderBy(b => b.DenormPrimaryAuthor);
        //            break;
        //        case "date_download_asc":
        //            matchList = matchList.OrderBy(b => b.DenormDownloadDate);
        //            break;
        //        case "date_download_desc":
        //            matchList = matchList.OrderByDescending(b => b.DenormDownloadDate);
        //            break;
        //    }


        //    // Step three: filter based on search. Blank searches are special.
        //    if (searchOperations == null)
        //    {
        //        var newlist = new List<Book>();
        //        enumerableList = newlist;
        //        foreach (var book in matchList)
        //        {
        //            if (true || !mustIncludeEpub // turn this off until it works; the .Files are always the same
        //                || Book.FilesIncludesEpub(book))
        //            {
        //                newlist.Add(book);
        //                if (newlist.Count > MaxMatch)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var newlist = new List<Book>();
        //        enumerableList = newlist;
        //        foreach (var book in matchList)
        //        {
        //            var epubMatch = true; // turn this off until it works !mustIncludeEpub || Book.FilesIncludesEpub(book);
        //            if (epubMatch && searchOperations.Matches(book))
        //            {
        //                newlist.Add(book);
        //                if (newlist.Count > MaxMatch) // gotta end early :-(
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }


        //    // This seems like a lot of code just to distinguish between
        //    // "the search returned 150 books" and "the search returned
        //    // 150 books and there are more if needed."
        //    andMore = false;
        //    var nmatch = 0;

        //    nmatch = 0;
        //    foreach (var book in enumerableList)
        //    {
        //        nmatch++;
        //        if (nmatch < MaxMatch) resultList.Add(book);
        //        else if (nmatch > MaxMatch)
        //        {
        //            andMore = true;
        //            break;
        //        }

        //    }
        //    return resultList;
        //}
    }
}
