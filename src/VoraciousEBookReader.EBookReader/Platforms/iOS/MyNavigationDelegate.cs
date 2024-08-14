using System.Text.RegularExpressions;

using Foundation;

using WebKit;

public class MyNavigationDelegate : WKNavigationDelegate
{
    private static readonly Regex _fileNameRegex = new("filename\\*?=['\"]?(?:UTF-\\d['\"]*)?([^;\\r\\n\"']*)['\"]?;?", RegexOptions.Compiled);

    public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
    {
        // Can't navigate away from the main window
        if (navigationAction.TargetFrame?.MainFrame != true)
        {
            // Cancel the original action and load the same request in the web view
            decisionHandler?.Invoke(WKNavigationActionPolicy.Cancel);
            webView.LoadRequest(navigationAction.Request);
            return;
        }

        decisionHandler?.Invoke(WKNavigationActionPolicy.Allow);
    }

    public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, WKWebpagePreferences preferences, Action<WKNavigationActionPolicy, WKWebpagePreferences> decisionHandler)
    {
        // Can't navigate away from the main window
        if (navigationAction.TargetFrame?.MainFrame != true)
        {
            // Cancel the original action and load the same request in the web view
            decisionHandler?.Invoke(WKNavigationActionPolicy.Cancel, preferences);
            webView.LoadRequest(navigationAction.Request);
            return;
        }

        decisionHandler?.Invoke(WKNavigationActionPolicy.Allow, preferences);
    }

    public override void DecidePolicy(WKWebView webView, WKNavigationResponse navigationResponse, Action<WKNavigationResponsePolicy> decisionHandler)
    {
        // Determine whether to treat it as a download
        if (navigationResponse.Response is NSHttpUrlResponse response
            && response.AllHeaderFields.TryGetValue(new NSString("Content-Disposition"), out var headerValue))
        {
            // Handle it as a download and prevent further navigation
            StartDownload(headerValue.ToString(), navigationResponse.Response.Url);
            decisionHandler?.Invoke(WKNavigationResponsePolicy.Cancel);
            return;
        }

        decisionHandler?.Invoke(WKNavigationResponsePolicy.Allow);
    }

    private void StartDownload(string contentDispositionHeader, NSUrl url)
    {
        try
        {
            var message = TryGetFileNameFromContentDisposition(contentDispositionHeader, out var fileName)
                ? $"Downloading {fileName}..."
                : "Downloading...";

            // TODO: Show toast message

            NSUrlSession
                .FromConfiguration(NSUrlSessionConfiguration.DefaultSessionConfiguration, new MyDownloadDelegate(), null)
                .CreateDownloadTask(url)
                .Resume();
        }
        catch (NSErrorException ex)
        {
            // TODO: Show toast message
        }
    }

    private bool TryGetFileNameFromContentDisposition(string contentDisposition, out string fileName)
    {
        if (string.IsNullOrEmpty(contentDisposition))
        {
            fileName = null;
            return false;
        }

        var match = _fileNameRegex.Match(contentDisposition);
        if (!match.Success)
        {
            fileName = null;
            return false;
        }

        // Use first match even though there could be several matched file names
        fileName = match.Groups[1].Value;
        return true;
    }
}