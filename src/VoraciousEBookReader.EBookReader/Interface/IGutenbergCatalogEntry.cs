using System.Collections.ObjectModel;
using System.Globalization;

namespace VoraciousEBookReader.EBookReader.Interface;

public interface IGutenbergCatalogEntry
{
    /// <summary>
    /// The number of the book in the catalog
    /// </summary>
    int BookNumber { get; set; }

    /// <summary>
    /// The Library of Congress Catalog number
    /// </summary>
    string BookLOCC {  get; set; }

    /// <summary>
    /// The title of the book
    /// </summary>
    string BookTitle {  get; set; }

    /// <summary>
    /// The type of publication
    /// </summary>
    string BookEPubType { get; set; }

    /// <summary>
    /// The date the book was issued on the site
    /// </summary>
    DateTime BookDateIssued { get; set; }

    /// <summary>
    /// The language(s) of the books as cultureInfo entries
    /// </summary>
    ObservableCollection<CultureInfo> BookLanguages {  get; set; }

    /// <summary>
    /// The subjects(s) of the book
    /// </summary>
    ObservableCollection<string> BookSubjects { get; set; }

    /// <summary>
    /// The book shelves the book is on
    /// </summary>
    ObservableCollection<string> BookShelves { get; set; }

    /// <summary>
    /// The authors of the book
    /// </summary>
    ObservableCollection<string> BookAuthors {  get; set; } 
}