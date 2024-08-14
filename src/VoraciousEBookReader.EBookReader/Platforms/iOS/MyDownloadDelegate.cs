using CoreFoundation;

using Foundation;

using UIKit;

using UniformTypeIdentifiers;

public class MyDownloadDelegate : NSUrlSessionDownloadDelegate
{
    public override void DidFinishDownloading(NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
    {
        try
        {
            if (downloadTask.Response == null)
            {
                return;
            }

            // Determine the cache folder
            var fileManager = NSFileManager.DefaultManager;
            var tempDir = fileManager.GetUrls(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User).FirstOrDefault();
            if (tempDir == null)
            {
                return;
            }

            var contentType = UTType.CreateFromMimeType(downloadTask.Response.MimeType);
            if (contentType == null)
            {
                return;
            }

            // Determine the file name in the cache folder
            var destinationPath = tempDir.AppendPathComponent(downloadTask.Response.SuggestedFilename, contentType);
            if (destinationPath == null || string.IsNullOrEmpty(destinationPath.Path))
            {
                return;
            }

            // Remove any existing files with the same name
            if (fileManager.FileExists(destinationPath.Path) && !fileManager.Remove(destinationPath, out var removeError))
            {
                return;
            }

            // Copy the downloaded file from the OS temp folder to our cache folder
            if (!fileManager.Copy(location, destinationPath, out var copyError))
            {
                return;
            }

            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                ShowFileOpenDialog(destinationPath);
            });
        }
        catch (NSErrorException ex)
        {
            // TODO: Show toast message
        }
    }

    private void ShowFileOpenDialog(NSUrl fileUrl)
    {
        try
        {
            var window = UIApplication.SharedApplication.Windows.Last(x => x.IsKeyWindow);

            var viewController = window.RootViewController;
            if (viewController == null || viewController.View == null)
            {
                return;
            }

            // TODO: Apps sometimes cannot open the file
            var documentController = UIDocumentInteractionController.FromUrl(fileUrl);
            documentController.PresentOpenInMenu(viewController.View.Frame, viewController.View, true);
        }
        catch (NSErrorException ex)
        {
            // TODO: Show toast message
        }
    }
}
