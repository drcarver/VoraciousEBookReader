using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using VoraciousEBookReader.Gutenberg;
using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReaderConsole;

internal class Program
{
    private const string PGCATALOG = "GutenbergCatalog.json";

    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader";

        builder.Environment.ContentRootPath = fPath;
        builder.Configuration.AddJsonFile("hostsettings.json", optional: true);
        builder.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
        builder.Configuration.AddCommandLine(args);

        builder.Services
            .AddHttpClient()
            .UseGutenbergCatalog();

        using IHost host = builder.Build();

        await host.StartAsync();

        var app = host.Services.GetRequiredService<ICatalog>();

        await host.RunAsync();
    }
}

//string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}";
//var fileInfo = new FileInfo(fPath);
//Directory.CreateDirectory(fileInfo?.DirectoryName);
//if (!File.Exists(fPath + ".gz"))
//{
//    var sw = new Stopwatch();
//    var catalog = new CatalogViewModel(null);
//    sw.Start();
//    await catalog();
//    sw.Stop();
//}

//// now read it back
//var sw3 = new Stopwatch();
//sw3.Start();
//var list = new List<CatalogEntry>();
//using (FileStream compressedFileStream = File.Open(fPath + ".gz", FileMode.Open))
//{
//    using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
//    {
//        list = JsonSerializer.Deserialize<List<CatalogEntry>>(decompressionStream);
//    }
//}
//sw3.Stop();
////File.Delete(fPath + ".gz");
//Console.WriteLine("Hello, World!");
