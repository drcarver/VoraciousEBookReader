using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.Configuration;

using VoraciousEBookReader.Gutenberg.Map;
using VoraciousEBookReader.Gutenberg.Model;

namespace VoraciousEBookReader.Gutenberg
{
    /// <summary>
    /// The Gutenberg catalog
    /// </summary>
    public class Catalog
    {
        /// <summary>
        /// Read the Gutenberg catalog
        /// </summary>
        /// <returns>The catalog as a list of catalog entries</returns>
        /// <exception cref="Exception">A exception occurred processing the catalog</exception>
        public async Task<List<CatalogEntry>> ReadCatalog()
        {
            try
            {
                string url = "https://www.gutenberg.org/cache/epub/feeds/pg_catalog.csv";
                using (var streamReader = new StreamReader(await new HttpClient().GetStreamAsync(url)))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true, Delimiter = ",", BadDataFound = null, MissingFieldFound = null };
                    using (var csv = new CsvReader(streamReader, config))
                    {
                        csv.Context.RegisterClassMap<CatalogEntryMap>();
                        return csv.GetRecords<CatalogEntry>().ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
