using System.Collections.ObjectModel;

using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Interface;

public interface ICatalog
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    ObservableCollection<CatalogEntryViewModel>? CatalogEntries { get; set; }

    /// <summary>
    /// The date and time the catalog was last downloaded
    /// </summary>
    DateTime LastUpdated { get; set; }
}