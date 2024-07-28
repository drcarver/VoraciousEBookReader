namespace VoraciousEBookReader.Gutenberg.Model;

public class GutenbergCatalog
{
    /// <summary>
    /// The catalog.  These are refreshed every week 
    /// </summary>
    public List<CatalogEntry> Catalog { get; set;  } = [];

    /// <summary>
    /// The date and time the catalog was downloaded
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public GutenbergCatalog()
    {

    }

}
