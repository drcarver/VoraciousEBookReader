using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.Model;

namespace VoraciousEBookReader.Gutenberg.ViewModel;

public partial class GutenbergCatalogViewModel : ObservableObject, ICatalog
{
    /// <summary>
    /// The catalog.  These are refreshed every week 
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GutenbergCatalogEntryViewModel> catalog = [];

    /// <summary>
    /// The date and time the catalog was downloaded
    /// </summary>
    [ObservableProperty]
    private DateTime lastUpdated;

    /// <summary>
    /// Return the Last Updated formatted as a long string
    /// </summary>
    public string LastUpdatedString => LastUpdated.ToString("D",
                  CultureInfo.CreateSpecificCulture("en-US"));

    /// <summary>
    /// The available book shelves
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> bookShelves = [];

    /// <summary>
    /// The available subjects
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> catalogSubjects = [];
}
