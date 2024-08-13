using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.EBookReader.ViewModel;

public partial class WebViewModel : ObservableObject, IQueryAttributable, IWebViewModel
{
    private readonly string BaseURI= "https://www.gutenberg.org/ebooks/";

    /// <summary>
    /// The source url
    /// </summary>
    [ObservableProperty]
    private string sourceURL;

    /// <summary>
    /// The query attributes from navigation
    /// </summary>
    /// <param name="query">The query dictionary</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        GutenbergCatalogEntryViewModel data = (GutenbergCatalogEntryViewModel)query["data"];
        SourceURL = $"{BaseURI}/{data.BookNumber}/";
    }
}
