using System.Collections.ObjectModel;
using System.Globalization;

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
    Dictionary<CultureInfo, List<GutenbergCatalogEntryViewModel>> Languages { get; }

    /// <summary>
    /// The languages selected from the category
    /// </summary>
    ObservableCollection<CultureInfo> SelectedLanguages { get; }
}