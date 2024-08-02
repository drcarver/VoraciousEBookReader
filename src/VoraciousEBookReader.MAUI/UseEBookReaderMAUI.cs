using CommunityToolkit.Maui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using VoraciousEBookReader.Gutenberg.ViewModel;
using VoraciousEBookReader.MAUI.View;

namespace VoraciousEBookReader.MAUI;

public static class DataServices
{
    /// <summary>
    /// The service collection extension for the Gutenberg catalog
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IServiceCollection UseEBookReaderMAUI(this IServiceCollection collection)
    {
        //collection
        // Add all the game table types as transient
        collection
            .AddSingletonWithShellRoute<GutenbergCatalogView, GutenbergCatalogViewModel>(nameof(GutenbergCatalogView));
        ;

        return collection;
    }
}
