using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.EBookReader.Interface;

namespace VoraciousEBookReader.Gutenberg.Interface;

public partial class GutenbergCatalogEntryViewModel : ObservableObject, IGutenbergCatalogEntry
{
    /// <summary>
    /// The authors of the book
    /// </summary>
    [ObservableProperty]
    private string author;

    /// <summary>
    /// The book shelves the book is on
    /// </summary>
    [ObservableProperty]
    private string shelf;

    /// <summary>
    /// The language of the book as a string
    /// </summary>
    [ObservableProperty]
    private string language;

    /// <summary>
    /// The Subjects of the book
    /// </summary>
    [ObservableProperty]
    private string subject;

    /// <summary>
    /// The number of the book in the catalog
    /// </summary>
    [ObservableProperty]
    private int bookNumber;

    /// <summary>
    /// The Library of Congress Catalog number
    /// </summary>
    [ObservableProperty]
    private string bookLOCC;

    /// <summary>
    /// The title of the book
    /// </summary>
    [ObservableProperty]
    private string bookTitle;

    /// <summary>
    /// The type of publication
    /// </summary>
    [ObservableProperty]
    private string bookEPubType;

    /// <summary>
    /// The date the book was issued on the site
    /// </summary>
    [ObservableProperty]
    private DateTime bookDateIssued;

    /// <summary>
    /// The language(s) of the books as cultureInfo entries
    /// </summary>
    [ObservableProperty]
    //[property: JsonIgnore]
    private ObservableCollection<CultureInfo> bookLanguages = [];

    /// <summary>
    /// The subjects(s) of the book
    /// </summary>
    [ObservableProperty]
    //[property: JsonIgnore]
    private ObservableCollection<string> bookSubjects = [];

    /// <summary>
    /// The book shelves the book is on
    /// </summary>
    [ObservableProperty]
    //[property: JsonIgnore]
    private ObservableCollection<string> bookShelves = [];

    /// <summary>
    /// The authors of the book
    /// </summary>
    [ObservableProperty]
    //[property: JsonIgnore]
    private ObservableCollection<string> bookAuthors = [];

    /// <summary>
    /// The raw book authors
    /// </summary>
    /// <param name="rawAuthors">The book authors as a string</param>
    partial void OnAuthorChanged(string? rawAuthors)
    {
        // The raw subject string
        if (string.IsNullOrEmpty(rawAuthors?.Trim()))
        {
            return;
        }

        var individualAuthors = rawAuthors?.Split(';');
        if (individualAuthors != null)
        {
            foreach (var individualAuthor in individualAuthors)
            {
                var key = individualAuthor.ToUpper().Trim();
                if (!string.IsNullOrEmpty(key) && !BookAuthors.Contains(key))
                {
                    BookAuthors.Add(key);
                }
            }
        }
    }

    /// <summary>
    /// The raw book subjects
    /// </summary>
    /// <param name="rawSubjects">The book subjects as a string</param>
    partial void OnSubjectChanged(string? rawSubjects)
    {
        // The raw subject string
        if (string.IsNullOrEmpty(rawSubjects?.Trim()))
        {
            return;
        }

        // Process the subjects
        var individualSubjects = rawSubjects?.Split(';');
        if (individualSubjects != null) 
        {
            foreach (var individualSubject in individualSubjects)
            {
                var key = individualSubject.ToUpper().Trim();
                if (!string.IsNullOrEmpty(key) && !BookSubjects.Contains(key))
                {
                    BookSubjects.Add(key);
                }
            }
        }
    }

    /// <summary>
    /// The raw book shelves
    /// </summary>
    /// <param name="rawBookshelves">The book shelves the book is on.</param>
    partial void OnShelfChanged(string? rawBookshelves)
    {
        // The raw subject string
        if (string.IsNullOrEmpty(rawBookshelves?.Trim()))
        {
            return;
        }

        // Process the BookShelves
        var individualBookShelves = rawBookshelves?.Split(';');
        if (individualBookShelves != null)
        {
            foreach (var individualSubject in individualBookShelves)
            {
                var key = individualSubject.ToUpper().Trim();
                if (!string.IsNullOrEmpty(key) && !BookShelves.Contains(key))
                {
                    BookShelves.Add(key);
                }
            }
        }
    }

    /// <summary>
    /// Convert the string version of the books languages to a list of CultureInfo  
    /// </summary>
    /// <param name="rawLanguages">The value of the language</param>
    partial void OnLanguageChanged(string? rawLanguages)
    {
        // The raw language string
        if (string.IsNullOrEmpty(rawLanguages?.Trim()))
        {
            return;
        }

        // Process the language(s) of the book
        try
        {
            var individualLanguages = rawLanguages?.Split(';');
            if (individualLanguages.Any()  & !string.IsNullOrEmpty(rawLanguages))
            {
                foreach (var individualLanguage in individualLanguages)
                {
                    var keyString = individualLanguage.ToUpper().Trim();
                    if (string.IsNullOrEmpty(keyString))
                    {
                        keyString = "en";
                    }
                    var key = CultureInfo.CreateSpecificCulture(keyString);
                    if (key != null && !BookLanguages.Contains(key))
                    {
                        BookLanguages.Add(key);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}
