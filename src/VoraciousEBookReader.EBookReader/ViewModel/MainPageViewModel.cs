using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.EBookReader.ViewModel;

public partial class MainPageViewModel : ObservableObject, IMainPageViewModel
{
    /// <summary>
    /// The Logger interface
    /// </summary>
    private ILogger Logger { get; }

    /// <summary>
    /// The Catalog service
    /// </summary>
    [ObservableProperty]
    private IGutenbergCatalogService catalogService;

    /// <summary>
    /// THe title of the application
    /// </summary>
    [ObservableProperty]
    private string title = "Voracious Ebook Reader";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger facade</param>
    /// <param name="service">The catalog service</param>
    public MainPageViewModel(
        ILoggerFactory loggerFactory,
        IGutenbergCatalogService service) 
    {
        Logger = loggerFactory.CreateLogger<MainPage>();
        CatalogService = service;
    }
}
