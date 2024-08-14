using Android.App;
using Android.Content;
using Android.Widget;

public class MyBroadcastReceiver : BroadcastReceiver
{
    private readonly long _downloadId;

    public MyBroadcastReceiver(long downloadId)
    {
        _downloadId = downloadId;
    }

    public override void OnReceive(Context context, Intent intent)
    {
        // Only handle download broadcasts
        if (intent.Action == DownloadManager.ActionDownloadComplete)
        {
            var downloadId = intent.GetLongExtra(DownloadManager.ExtraDownloadId, 0);

            // Only handle specific download ID
            if (downloadId == _downloadId)
            {
                OpenFile(context, downloadId);
                context.UnregisterReceiver(this);
            }
        }
    }

    private void OpenFile(Context context, long downloadId)
    {
        var downloadManager = (DownloadManager)context.GetSystemService(Context.DownloadService);
        var fileUri = downloadManager.GetUriForDownloadedFile(downloadId);
        var fileMimeType = downloadManager.GetMimeTypeForDownloadedFile(downloadId);

        if (fileUri == null || fileMimeType == null)
        {
            return;
        }

        var viewFileIntent = new Intent(Intent.ActionView);
        viewFileIntent.SetDataAndType(fileUri, fileMimeType);
        viewFileIntent.SetFlags(ActivityFlags.GrantReadUriPermission);
        viewFileIntent.AddFlags(ActivityFlags.NewTask);

        try
        {
            context.StartActivity(viewFileIntent);
        }
        catch (Java.Lang.Exception ex)
        {
            Toast
                .MakeText(
                    context,
                    $"Unable to open file: {ex.Message}",
                    ToastLength.Long)
                .Show();
        }
    }
}