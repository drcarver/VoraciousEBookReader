using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using Syncfusion.Maui.DataGrid;

using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.EBookReader.View;
using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.EBookReader.ViewModel;

public partial class MainPageViewModel : BaseViewModel, IMainPageViewModel
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
    /// The height of the rows
    /// </summary>
    [ObservableProperty]
    private double rowHeight = 0;

    /// <summary>
    /// Process the cell tapped command
    /// </summary>
    /// <param name="e">The event arguments</param>
    /// <returns>The command Task</returns>
    [RelayCommand]
    private async Task CellTapped(DataGridCellTappedEventArgs e)
    {
        var data = (GutenbergCatalogEntryViewModel)e.RowData;
    }

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
    /// Go to the settings page 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task Settings()
    {
        await Shell.Current.GoToAsync(nameof(LanguageView));
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
