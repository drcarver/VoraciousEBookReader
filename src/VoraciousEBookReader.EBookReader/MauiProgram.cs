using CommunityToolkit.Maui;

using Microsoft.Extensions.Logging;

using Syncfusion.Maui.Core.Hosting;

using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.EBookReader.View;
using VoraciousEBookReader.EBookReader.ViewModel;
using VoraciousEBookReader.Gutenberg;
using VoraciousEBookReader.Gutenberg.Interface;

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
            .AddTransient<IWebViewModel, WebViewModel>()
            .AddSingleton<ILanguages, LanguageViewModel>()
            .AddSingleton<IMainPageViewModel, MainPageViewModel>()
            .AddSingletonWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPage))
            .AddSingletonWithShellRoute<LanguageView, LanguageViewModel>(nameof(LanguageView))
            .AddTransientWithShellRoute<WebViewPage, WebViewModel>(nameof(WebViewPage))
            .UseGutenbergCatalog();
        
        return builder.Build();
    }
}
