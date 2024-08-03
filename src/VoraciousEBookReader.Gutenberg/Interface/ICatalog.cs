using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.Gutenberg.Model;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Interface;

public interface ICatalog
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    ObservableCollection<GutenbergCatalogEntryViewModel> Catalog { get; set; }

    /// <summary>
    /// The date and time the catalog was last downloaded
    /// </summary>
    DateTime LastUpdated { get; set; }

    /// <summary>
    /// LastUpdated as a string
    /// </summary>
    string LastUpdatedString { get; }

    /// <summary>
    /// The available subjects
    /// </summary>
    ObservableCollection<string> CatalogSubjects { get; }

    /// <summary>
    /// The available book shelves
    /// </summary>
    ObservableCollection<string> BookShelves { get; }
}