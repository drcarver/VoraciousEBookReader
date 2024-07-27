using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.Extensions.Logging;

using VoraciousEBookReader.Gutenberg.Map;
using VoraciousEBookReader.Gutenberg.Model;

namespace VoraciousEBookReader.Gutenberg
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    public class Catalog
    {
        private const string PGCATALOG = "GutenbergCatalog.json";
        private readonly ILogger logger;
        public IHttpClientFactory HttpClientFactory { get; }

        public Catalog(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory) 
        {
            logger = loggerFactory.CreateLogger<Catalog>();
            HttpClientFactory = httpClientFactory;
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
                            CatalogEntries = JsonSerializer.Deserialize<List<CatalogEntry>>(decompressionStream);
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
                Task task = GetLatestCatalog();
            }
        }

        /// <summary>
        /// The Gutenberg catalog
        /// </summary>
        public List<CatalogEntry>? CatalogEntries { get; set; }

        /// <summary>
        /// When was the file was last updated?
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Read the Gutenberg catalog
        /// </summary>
        /// <returns>The catalog as a list of catalog entries</returns>
        /// <exception cref="Exception">A exception occurred processing the catalog</exception>
        public async Task GetLatestCatalog()
        {
            string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}";
            try
            {
                var fileInfo = new FileInfo(fPath);
                Directory.CreateDirectory(fileInfo?.DirectoryName);
                string url = "https://www.gutenberg.org/cache/epub/feeds/pg_catalog.csv";
                logger.LogInformation($"Loading catalog from {fPath}");
                List<CatalogEntry> list = new List<CatalogEntry>();
                HttpClient client = HttpClientFactory.CreateClient();
                using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, Delimiter = ",", BadDataFound = null, MissingFieldFound = null };
                    using (var csv = new CsvReader(streamReader, config))
                    {
                        csv.Context.RegisterClassMap<CatalogEntryMap>();
                        list = csv.GetRecords<CatalogEntry>().ToList();
                    }
                }
                LastUpdated = DateTime.Now;
                logger.LogInformation($"Loaded catalog with {list.Count} entries.");

                string json = JsonSerializer.Serialize(list);
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
                CatalogEntries = list;

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
