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
using VoraciousEBookReader.Gutenberg.Model;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.Gutenberg
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    public partial class CatalogViewModel : ObservableObject, ICatalog
    {
        // The catalog file name
        private const string PGCATALOG = "GutenbergCatalog.json";

        // The ILogger from the DI
        private readonly ILogger logger;

        // The httpclientfactory from the DI
        private IHttpClientFactory HttpClientFactory { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogViewModel()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClientFactory">The HTTP client factory</param>
        public CatalogViewModel(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory)
        {
            logger = loggerFactory.CreateLogger<CatalogViewModel>();
            HttpClientFactory = httpClientFactory;
            string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}.gz";
            if (File.Exists(fPath))
            {
                logger.LogInformation($"Loading catalog from {fPath}");
                CatalogEntries.Clear();
                try
                {
                    using (FileStream compressedFileStream = File.Open(fPath, FileMode.Open))
                    {
                        using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                        {
                             CatalogEntries = JsonSerializer.Deserialize<ObservableCollection<CatalogEntryViewModel>>(decompressionStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error {ex.Message} loading catalog from {fPath}");
                }
            }
            else
            {
                _ = GetLatestCatalog();
            }
        }

        /// <summary>
        /// The Gutenberg catalog
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<CatalogEntryViewModel>? catalogEntries = [];

        /// <summary>
        /// When was the file was last updated?
        /// </summary>
        [ObservableProperty]
        private DateTime lastUpdated;

        /// <summary>
        /// Read the Gutenberg catalog
        /// </summary>
        /// <returns>The catalog as a list of catalog entries</returns>
        /// <exception cref="Exception">A exception occurred processing the catalog</exception>
        [RelayCommand]
        private async Task GetLatestCatalog()
        {
            string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}";
            try
            {
                var fileInfo = new FileInfo(fPath);
                Directory.CreateDirectory(fileInfo?.DirectoryName);
                string url = "https://www.gutenberg.org/cache/epub/feeds/pg_catalog.csv";
                logger.LogInformation($"Loading catalog from {fPath}");
                List<CatalogEntry> list = [];
                HttpClient client = HttpClientFactory.CreateClient();
                using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, Delimiter = ",", BadDataFound = null, MissingFieldFound = null };
                    using (var csv = new CsvReader(streamReader, config))
                    {
                        csv.Context.RegisterClassMap<CatalogEntryMap>();
                        list = csv.GetRecords<CatalogEntry>().ToList<CatalogEntry>();
                    }
                }
                LastUpdated = DateTime.Now;
                foreach (var entry in list)
                {
                    CatalogEntries.Add(entry);
                }
                logger.LogInformation($"Loaded catalog with {list.Count} entries.");

                string json = JsonSerializer.Serialize(this);
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
                // delete the uncompressed list
                File.Delete(fPath);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error {ex.Message} loading catalog from {fPath}");
            }
        }
    }
}
