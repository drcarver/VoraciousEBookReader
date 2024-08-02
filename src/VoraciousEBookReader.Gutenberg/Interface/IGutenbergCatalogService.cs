using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Interface;

public interface IGutenbergCatalogService
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    GutenbergCatalogViewModel GutenbergCatalog { get; set; }

    /// <summary>
    /// Read the Gutenberg catalog
    /// </summary>
    /// <returns>The catalog as a list of catalog entries</returns>
    /// <exception cref="Exception">A exception occurred processing the catalog</exception>
    Task GetLatestCatalogAsync();

    ///// <summary>
    ///// Load the catalog from a local file
    ///// </summary>
    ///// <returns>The task for the async method</returns>
    Task LoadLocalCatalogAsync();
}
