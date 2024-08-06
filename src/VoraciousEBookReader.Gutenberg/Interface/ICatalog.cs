using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;

using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Interface;

public interface ICatalog
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    ObservableCollection<GutenbergCatalogEntryViewModel> Catalog { get; set; }

    /// <summary>
    /// Create the indexes of the catalog
    /// </summary>
    public void CreateIndexes();

    /// <summary>
    /// The date and time the catalog was last downloaded
    /// </summary>
    DateTime LastUpdated { get; set; }

    /// <summary>
    /// The available Languages
    /// </summary>
    Dictionary<CultureInfo, List<GutenbergCatalogEntryViewModel>> Languages { get; set; }

    /// <summary>
    /// The available book shelves
    /// </summary>
    Dictionary<string, List<GutenbergCatalogEntryViewModel>> Shelves { get; set; }

    /// <summary>
    /// The available subjects
    /// </summary>
    Dictionary<string, List<GutenbergCatalogEntryViewModel>> Subjects { get; set; }

    /// <summary>
    /// The available authors
    /// </summary>
    Dictionary<string, List<GutenbergCatalogEntryViewModel>> Authors { get; set; }
}