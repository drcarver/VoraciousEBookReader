using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Interface;

public interface ILanguages 
{
    /// <summary>
    /// The title of the view
    /// </summary>
    string Title { get; }

    /// <summary>
    /// The books in each language in the catalog
    /// </summary>
    Dictionary<CultureInfo, ObservableCollection<GutenbergCatalogEntryViewModel>> Languages { get; }

    /// <summary>
    /// The languages selected from the category
    /// </summary>
    ObservableCollection<CultureInfo> SelectedLanguages { get; }

    /// <summary>
    /// The list of selected books
    /// </summary>
    ObservableCollection<GutenbergCatalogEntryViewModel> SelectedEntries { get; set; }
}