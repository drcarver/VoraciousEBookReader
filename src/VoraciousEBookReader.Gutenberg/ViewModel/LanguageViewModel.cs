using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.EBookReader.Interface;

namespace VoraciousEBookReader.Gutenberg.ViewModel;

public partial class LanguageViewModel : ObservableObject, ILanguages
{
    /// <summary>
    /// The languages in the catalog
    /// </summary>
    [ObservableProperty]
    private Dictionary<CultureInfo, List<GutenbergCatalogEntryViewModel>> languages = [];

    /// <summary>
    /// The Languages selected from the catalog
    /// </summary>
    public ObservableCollection<CultureInfo> SelectedLanguages { get; } = [];
}
