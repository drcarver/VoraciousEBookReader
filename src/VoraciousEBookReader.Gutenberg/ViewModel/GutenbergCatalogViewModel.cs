using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.EBookReader.ViewModel;

public partial class GutenbergCatalogViewModel : BaseViewModel, ICatalog
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    public GutenbergCatalogViewModel(
        ILoggerFactory loggerFactory,
        ILanguages languages)
    {
        logger = loggerFactory.CreateLogger<GutenbergCatalogViewModel>();
        LanguagesInCatalog = languages;
    }

    /// <summary>
    /// Add the item to shelves dictionary
    /// </summary>
    /// <param name="item">The item to add</param>
    private void AddShelves(GutenbergCatalogEntryViewModel item)
    {

        foreach (var shelf in item.BookShelves)
        {
            var key = shelf.ToUpper().Trim();
            if (!Shelves.ContainsKey(key))
            {
                Shelves.Add(key, []);
            }
            Shelves[key].Add(item);
        }
    }

    /// <summary>
    /// Add the item to the languages dictionary
    /// </summary>
    /// <param name="item">The item to add</param>
    private void AddLanguages(GutenbergCatalogEntryViewModel item)
    {
        LanguagesInCatalog.SelectedEntries.Clear();
        foreach (var key in item.BookLanguages)
        {
            if (!LanguagesInCatalog.Languages.ContainsKey(key))
            {
                LanguagesInCatalog.Languages.Add(key, []);
            }
            LanguagesInCatalog.Languages[key].Add(item);
        }
    }

    /// <summary>
    /// Add the item to the authors dictionary
    /// </summary>
    /// <param name="item">The item to add</param>
    private void AddAuthors(GutenbergCatalogEntryViewModel item)
    {
        foreach (var author in item.BookAuthors)
        {
            var key = author.ToUpper().Trim();
            if (!Authors.ContainsKey(key))
            {
                Authors.Add(key, []);
            }
            Authors[key].Add(item);
        }
    }

    /// <summary>
    /// Add the subjects to the subjects dictionary
    /// </summary>
    /// <param name="item">The subject to add</param>
    private void AddSubjects(GutenbergCatalogEntryViewModel item)
    {
        foreach (var subject in item.BookSubjects)
        {
            var key = subject.ToUpper().Trim();
            if (!Subjects.ContainsKey(key))
            {
                Subjects.Add(key, []);
            }
            Subjects[key].Add(item);
        }

    }

    /// <summary>
    /// Create the index for the catalogs
    /// </summary>
    public void CreateIndexes()
    {
        // Create the index for authors
        foreach (var item in Catalog.Where(i => i.BookAuthors.Any()))
        {
            AddAuthors(item);
        }

        // Create the languages index
        foreach (var item in Catalog.Where(i => i.BookLanguages.Any()))
        {
            AddLanguages(item);
        }
        LanguagesInCatalog.SelectedLanguages.Clear();
        var english = new CultureInfo("en-US");
        LanguagesInCatalog.SelectedLanguages.Add(english);
        LanguagesInCatalog.SelectedEntries = LanguagesInCatalog.Languages[english];

        // Create the subjects index
        foreach (var item in Catalog.Where(i => i.BookSubjects.Any()))
        {
            AddSubjects(item);
        }

        // Create the subjects index
        foreach (var item in Catalog.Where(i => i.BookShelves.Any()))
        {
            AddShelves(item);
        }
    }

    /// <summary>`
    /// The logger 
    /// </summary>
    private ILogger<GutenbergCatalogViewModel> logger;

    /// <summary>
    /// The catalog.  These are refreshed every week 
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GutenbergCatalogEntryViewModel> catalog;

    /// <summary>
    /// The date and time the catalog was downloaded
    /// </summary>
    [ObservableProperty]
    private DateTime lastUpdated = DateTime.Now;

    /// <summary>
    /// The available Languages
    /// </summary>
    [ObservableProperty]
    private ILanguages languagesInCatalog;

    /// <summary>
    /// The available book shelves
    /// </summary>
    [ObservableProperty]
    private Dictionary<string, List<GutenbergCatalogEntryViewModel>> shelves = [];

    /// <summary>
    /// The available subjects
    /// </summary>
    [ObservableProperty]
    private Dictionary<string, List<GutenbergCatalogEntryViewModel>> subjects = [];

    /// <summary>
    /// The available authors
    /// </summary>
    [ObservableProperty]
    private Dictionary<string, List<GutenbergCatalogEntryViewModel>> authors = [];
}
