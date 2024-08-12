using System.Collections.ObjectModel;
using System.Globalization;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.EBookReader.ViewModel;

public partial class LanguageViewModel : ObservableObject, ILanguages
{
    /// <summary>
    /// The languages in the catalog
    /// </summary>
    [ObservableProperty]
    private Dictionary<CultureInfo, ObservableCollection<GutenbergCatalogEntryViewModel>> languages = [];

    /// <summary>
    /// The Languages selected from the catalog
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<CultureInfo> selectedLanguages = [];

    /// <summary>
    /// The list of selected books
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GutenbergCatalogEntryViewModel> selectedEntries = [];

    /// <summary>
    /// The books in the selected language
    /// </summary>
    /// <param name="value">The book language</param>
    partial void OnSelectedLanguagesChanged(ObservableCollection<CultureInfo> value)
    {
        IEnumerable<GutenbergCatalogEntryViewModel> listEntries;
        foreach (var ci in value)
        {
            listEntries = Languages[ci].OrderBy(o => o.BookTitle.ToUpper().Trim());
            foreach (var entry in listEntries)
            {
                if (!SelectedEntries.Contains(entry))
                {
                    SelectedEntries.Add(entry);
                }
            }
        }
    }

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
