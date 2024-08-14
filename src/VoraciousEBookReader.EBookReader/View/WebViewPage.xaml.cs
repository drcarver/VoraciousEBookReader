using VoraciousEBookReader.EBookReader.Interface;

namespace VoraciousEBookReader.EBookReader.View;

public partial class WebViewPage : ContentPage
{
    public WebViewPage(IWebViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    /// <summary>
    /// Handle for file download
    /// </summary>
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
#if IOS
        var iosWebView = WebView.Handler.PlatformView as WebKit.WKWebView;

        // Otherwise swiping doesn't work
        iosWebView.AllowsBackForwardNavigationGestures = true;
#endif

#if ANDROID
        var androidWebView = WebView.Handler.PlatformView as Android.Webkit.WebView;

        // If this is not disabled then download links that open in a new tab won't work
        androidWebView.Settings.SetSupportMultipleWindows(false);

        // Custom download listener for Android
        androidWebView.SetDownloadListener(new MyDownloadListener());
#endif
    }
}