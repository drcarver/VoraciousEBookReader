using System.Diagnostics;
using System.IO;

using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;

using Microsoft.Extensions.Logging;

using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.EBookReader.ViewModel;
using VoraciousEBookReader.Gutenberg;

namespace VoraciousEBookReader.EBookReader;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        //Add a custom log provider to write logs to text files
        var dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\";
        Directory.CreateDirectory(dir);
        string fPath = $"{dir}\\EBookReader.logfile.txt";
        builder.Logging.AddProvider(new FileLogger(fPath));

#if DEBUG
        builder.Logging
            .AddDebug();
#endif

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services
            .AddHttpClient()
            .AddSingleton<IMainPageViewModel, MainPageViewModel>()
            .AddSingletonWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPage))
            .UseGutenbergCatalog();
        
        return builder.Build();
    }
}
