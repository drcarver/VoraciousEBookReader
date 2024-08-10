using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.EBookReader.ViewModel;

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
    [ObservableProperty]
    private ObservableCollection<CultureInfo> selectedLanguages = [];

    /// <summary>
    /// The title of the view
    /// </summary>
    [ObservableProperty]
    private string title = "Books are in available in the following Languages";

    /// <summary>
    /// The relay command for the search
    /// </summary>
    [RelayCommand]
    private async Task search(string searchFilter)
    {

    }
}
