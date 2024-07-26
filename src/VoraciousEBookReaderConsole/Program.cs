using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using VoraciousEBookReader.Gutenberg;

using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System.Runtime.InteropServices.JavaScript;
using VoraciousEBookReader.Gutenberg.Model;

namespace VoraciousEBookReaderConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var filePath = @"C:\Users\drcarver\Desktop\VoraciousEBookReader\src\VoraciousEBookReaderConsole\catalog.json";
        if (!File.Exists(filePath))
        {
            var sw = new Stopwatch();
            var catalog = new Catalog();
            sw.Start();
            var catalogList = await catalog.ReadCatalog();
            sw.Stop();

            // Serialize to JSON and write to a file
            var sw2 = new Stopwatch();
            sw2.Start();
            string json = JsonSerializer.Serialize(catalogList);
            File.WriteAllText(filePath, json);
            sw2.Stop();
        }

        // now read it back
        var sw3 = new Stopwatch();
        sw3.Start();
        var catList = JsonSerializer.Deserialize<List<CatalogEntry>>(File.ReadAllText(filePath));
        sw3.Stop();
        
        Console.WriteLine("Hello, World!");
    }
}
