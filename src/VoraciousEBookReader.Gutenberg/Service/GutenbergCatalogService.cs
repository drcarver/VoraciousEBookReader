using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.Extensions.Logging;

using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.Map;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg.Service;

/// <summary>
/// Load the catalog from disk or from the web
/// </summary>
public partial class GutenbergCatalogService : ObservableObject, IGutenbergCatalogService
{
    // The catalog file name
    private const string PGCATALOG = "GutenbergCatalog.json";

    // The file path to the catalog
    private readonly string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}";

    // The catalog url
    private const string CATALOGURL = "https://www.gutenberg.org/cache/epub/feeds/pg_catalog.csv";

    // The ILogger from the DI
    private readonly ILogger logger;

    // The HttpClientFactory from the DI
    private IHttpClientFactory HttpClientFactory { get; }

    /// <summary>
    /// The catalog entry
    /// </summary>
    [ObservableProperty]
    private ICatalog gutenbergCatalog;

    /// <summary>
    /// Read the Gutenberg catalog
    /// </summary>
    /// <returns>The catalog as a list of catalog entries</returns>
    /// <exception cref="Exception">A exception occurred processing the catalog</exception>
    [RelayCommand]
    private async Task GetLatestCatalog()
    {
        await GetLatestCatalogAsync();
    }

    /// <summary>
    /// Load the catalog from a local file
    /// </summary>
    /// <returns>The task for the async method</returns>
    /// <exception cref="Exception">A exception occurred processing the catalog</exception>
    [RelayCommand]
    private async Task LoadLocalCatalog()
    {
        await LoadLocalCatalogAsync();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="httpClientFactory">The HTTP client factory</param>
    /// <param name="catalog">The catalog</param>
    public GutenbergCatalogService(
        ILoggerFactory loggerFactory,
        IHttpClientFactory httpClientFactory,
        ICatalog catalog)
    {
        logger = loggerFactory.CreateLogger<GutenbergCatalogService>();
        HttpClientFactory = httpClientFactory;
        GutenbergCatalog = catalog;
    }

    /// <summary>
    /// Read the Gutenberg catalog
    /// </summary>
    /// <returns>The catalog as a list of catalog entries</returns>
    /// <exception cref="Exception">A exception occurred processing the catalog</exception>
    public async Task GetLatestCatalogAsync()
    {
        try
        {
            var fileInfo = new FileInfo(fPath);
            Directory.CreateDirectory(fileInfo?.DirectoryName);
            logger.LogInformation($"Loading catalog from {fPath}");
            HttpClient client = HttpClientFactory.CreateClient();
            ObservableCollection<GutenbergCatalogEntryViewModel> list = [];
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            { 
                HasHeaderRecord = true, 
                Delimiter = ",", 
                BadDataFound = null, 
                MissingFieldFound = null 
            };
            using (var csv = new CsvReader(new StreamReader(await client.GetStreamAsync(CATALOGURL)), config))
            {
                csv.Context.RegisterClassMap<GutenbergCatalogEntryViewModelMap>();
                GutenbergCatalog.Catalog = new ObservableCollection<GutenbergCatalogEntryViewModel>(csv.GetRecords<GutenbergCatalogEntryViewModel>());
            }
            GutenbergCatalog.LastUpdated = DateTime.Now;
            logger.LogInformation($"Loaded catalog with {GutenbergCatalog.Catalog.Count} entries.");

            // Process all the indexes of the catalog
            GutenbergCatalog.CreateIndexes();

            // save off the category collection
            string json = JsonSerializer.Serialize(GutenbergCatalog.Catalog);
            File.WriteAllText(fPath, json);
            using (var originalFileStream = File.Open(fPath, FileMode.Open))
            {
                using (var compressedFileStream = File.Create(fPath + ".gz"))
                {
                    logger.LogInformation($"Saved catalog to {fPath}.gz.");
                    using (var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressor);
                    }
                }
            }
            File.Delete(fPath);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error {ex.Message} loading catalog from {fPath}");
            throw ex;
        }
    }

    /// <summary>
    /// Load the catalog from a local file
    /// </summary>
    /// <returns>The task for the async method</returns>
    /// <exception cref="Exception">A exception occurred processing the catalog</exception>
    public async Task LoadLocalCatalogAsync() 
    {
        string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}.gz";
        if (File.Exists(fPath))
        {
            logger.LogInformation($"Loading catalog from {fPath}");
            try
            {
                using (FileStream compressedFileStream = File.Open(fPath, FileMode.Open))
                {
                    using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        GutenbergCatalog.Catalog = await JsonSerializer.DeserializeAsync<ObservableCollection<GutenbergCatalogEntryViewModel>>(decompressionStream);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error {ex.Message} loading catalog from {fPath}");
                throw ex;
            }
            FileInfo fInfo = new FileInfo(fPath);
            GutenbergCatalog.LastUpdated = fInfo.LastWriteTime;
            GutenbergCatalog.CreateIndexes();
            logger.LogInformation($"Loaded catalog from {fPath}");
        }
        else
        {
            await GetLatestCatalog();
        }
    }
}
