using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.Gutenberg.ViewModel;

public partial class GutenbergCatalogEntryViewModel : ObservableObject, IGutenbergCatalogEntry
{
    [ObservableProperty]
    private string authors;

    [ObservableProperty]
    private string bookshelves;

    [ObservableProperty]
    private int ebookNumber;

    [ObservableProperty]
    private string ePubType;

    [ObservableProperty]
    private DateTime issued;

    [ObservableProperty]
    private string language;

    [ObservableProperty]
    private string loCC;

    [ObservableProperty]
    private string subjects;

    [ObservableProperty]
    private string title;
}
