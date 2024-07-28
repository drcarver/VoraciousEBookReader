namespace VoraciousEBookReader.Gutenberg.Model;

public class GutenbergCatalog
{
    /// <summary>
    /// The catalog.  These are refreshed every week 
    /// </summary>
    public List<GutenbergCatalogEntry> Catalog { get; set;  } = [];

    /// <summary>
    /// The date and time the catalog was downloaded
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
