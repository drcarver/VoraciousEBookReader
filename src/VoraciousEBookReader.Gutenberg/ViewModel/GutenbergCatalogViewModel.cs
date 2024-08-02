using System.Collections.ObjectModel;

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
}
