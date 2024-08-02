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
            .UseGutenbergCatalog();

        using IHost host = builder.Build();

        await host.StartAsync();

        var app = host.Services.GetRequiredService<ICatalog>();

        await host.RunAsync();
    }
}
