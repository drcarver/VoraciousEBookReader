using System.Collections.ObjectModel;
using System.Globalization;

using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.EBookReader.Interface;

public interface ILanguages
{
    /// <summary>
    /// The books in each language in the catalog
    /// </summary>
    Dictionary<CultureInfo, List<GutenbergCatalogEntryViewModel>> Languages { get; }

    /// <summary>
    /// The languages selected from the category
    /// </summary>
    ObservableCollection<CultureInfo> SelectedLanguages { get; }
}