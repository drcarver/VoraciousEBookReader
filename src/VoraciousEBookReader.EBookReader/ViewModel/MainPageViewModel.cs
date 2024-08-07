using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    /// The async command for the load catalog button
    /// </summary>
    [RelayCommand]
    private async Task LoadCatalog()
    {
        await CatalogService.LoadLocalCatalogAsync();
        if (CatalogService.GutenbergCatalog.Catalog.Any())
        {
            LoadCommandText = "Click to Refresh Catalog";
        }
    }

    /// <summary>
    /// The text for the load command
    /// </summary>
    [ObservableProperty]
    private string loadCommandText = "Click to Load Catalog";

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
