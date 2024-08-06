using Microsoft.Extensions.DependencyInjection;

using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.Service;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg
{
    public static class DataServices
    {
        /// <summary>
        /// The service collection extension for the Gutenberg catalog
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IServiceCollection UseGutenbergCatalog(this IServiceCollection collection)
        {
            //collection
            // Add all the game table types as transient
            collection
                .AddTransient<IGutenbergCatalogEntry, GutenbergCatalogEntryViewModel>()
                .AddSingleton<IGutenbergCatalogService, GutenbergCatalogService>()
                .AddSingleton<ICatalog, GutenbergCatalogViewModel>()
                ;

            return collection;
        }
    }
}
