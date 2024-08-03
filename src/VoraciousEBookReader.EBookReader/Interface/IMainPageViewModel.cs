using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.EBookReader.Interface;

public interface IMainPageViewModel
{
    /// <summary>
    /// Page title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Catalog service
    /// </summary>
    IGutenbergCatalogService CatalogService { get; }
}