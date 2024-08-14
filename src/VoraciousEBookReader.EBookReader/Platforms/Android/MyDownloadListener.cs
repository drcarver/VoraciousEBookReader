using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.Webkit;
using Android.Widget;

public class MyDownloadListener : Java.Lang.Object, IDownloadListener
{
    private readonly Regex _fileNameRegex = new("filename\\*?=['\"]?(?:UTF-\\d['\"]*)?([^;\\r\\n\"']*)['\"]?;?", RegexOptions.Compiled);

    public void OnDownloadStart(string url, string userAgent, string contentDisposition, string mimetype, long contentLength)
    {
        if (!TryGetFileNameFromContentDisposition(contentDisposition, out var fileName))
        {
            // GuessFileName doesn't work well, use it as a fallback
            fileName = URLUtil.GuessFileName(url, contentDisposition, mimetype);
        }

        var text = $"Downloading {fileName}...";
        var uri = global::Android.Net.Uri.Parse(url);
        var context = Platform.CurrentActivity?.Window?.DecorView.FindViewById(global::Android.Resource.Id.Content)?.RootView?.Context;

        try
        {
            var request = new DownloadManager.Request(uri);
            request.SetTitle(fileName);
            request.SetDescription(text);
            request.SetMimeType(mimetype);
            request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);

            // File should be saved in public downloads so that it can be opened without extra effort
            request.SetDestinationInExternalPublicDir(global::Android.OS.Environment.DirectoryDownloads, fileName);

            // Cookies have to be copied, otherwise authorized files won't download
            var cookie = CookieManager.Instance.GetCookie(url);
            request.AddRequestHeader("Cookie", cookie);

            var downloadManager = (DownloadManager)Platform.CurrentActivity.GetSystemService(Context.DownloadService);
            var downloadId = downloadManager.Enqueue(request);

            if (ShouldOpenFile(contentDisposition))
            {
                // Receiver will open the file after the download has finished
                context.RegisterReceiver(new MyBroadcastReceiver(downloadId), new IntentFilter(DownloadManager.ActionDownloadComplete));
            }

            Toast
                .MakeText(
                    context,
                    text,
                    ToastLength.Short)
                .Show();
        }
        catch (Java.Lang.Exception ex)
        {
            Toast
                .MakeText(
                    context,
                    $"Unable to download file: {ex.Message}",
                    ToastLength.Long)
                .Show();
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

    private bool ShouldOpenFile(string contentDisposition)
    {
        if (string.IsNullOrEmpty(contentDisposition))
        {
            return false;
        }

        return contentDisposition.StartsWith("inline", StringComparison.InvariantCultureIgnoreCase);
    }
}