using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace VoraciousEBookReader.Gutenberg
{
    public class Downloader
    {
        HttpClient hc = new HttpClient();

        /// <summary>
        /// A possibly long-running task to download a ZIP file contains a TAR file or RDF files
        /// the provide raw data for each book. See the RdfReader for the companion reader.
        /// TODO: it seems like after this runs, the database doesn't have a Books table and then 
        /// search doesn't work.
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<bool> DownloadFrom(IndexReader ui, Uri uri, StorageFolder folder, string filename, CancellationToken ct)
        {
            var retval = false;
            int totalRead = 0;
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            Stream stream = null;
            System.Diagnostics.Debug.WriteLine($"Note: downloading catalog from {uri.OriginalString}");
            System.Diagnostics.Debug.WriteLine($"Note: output file Path={file.Path} DisplayName={file.DisplayName}");
            using (var outstreamUwp = await file.OpenAsync(FileAccessMode.ReadWrite)) // (PCLStorage.FileAccess.ReadAndWrite))
            {
                var outstream = outstreamUwp.AsStreamForWrite();
                await ui.LogAsync($"Start download from {uri.OriginalString}\n");
                HttpResponseMessage result = null;
                const uint mbufferSize = 1024 * 1024;
                //var mbuffer = new Windows.Storage.Streams.Buffer(mbufferSize); // capacity = 1 megabyte
                var mbuffer = new byte[mbufferSize];
                try
                {
                    result = await hc.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, ct);
                    if (result.Content.Headers.ContentLength.HasValue)
                    {
                        ui.SetFileSize((ulong)result.Content.Headers.ContentLength.Value);
                        System.Diagnostics.Debug.WriteLine($"Note: downloading catalog size={result.Content.Headers.ContentLength.Value}");

                    }
                    stream = await result.Content.ReadAsStreamAsync();
                    // Just having the stream means nothing; we need to read from the stream. That's
                    // where the incoming bytes will actually be read
                }
                catch (Exception cancelex)
                {
                    ; // Throws on cancel
                    await ui.LogAsync($"Network error: {cancelex.Message}\n");
                }

                try
                {
                    bool keepGoing = !ct.IsCancellationRequested;
                    int tempTotalRead = 0;

                    while (keepGoing)
                    {
                        //var readTask = stream.ReadAsync(mbuffer, mbufferSize, Windows.Storage.Streams.InputStreamOptions.Partial)
                        //    .AsTask(ct, new Progress<uint>(async (readLen) => await ui.OnStreamProgressAsync(readLen)));
                        //var readBuffer = await readTask;
                        var nread = await stream.ReadAsync(mbuffer, 0, mbuffer.Length);
                        totalRead += nread;
                        tempTotalRead += nread;
                        if (nread == 0) // When we get no bytes, the stream is done.
                        {
                            System.Diagnostics.Debug.WriteLine($"Note: done reading size={totalRead}");
                            keepGoing = false;
                            retval = true;
                        }
                        else
                        {
                            // Write out
                            outstream.Write(mbuffer, 0, nread);
                        }
                        if (tempTotalRead > 1_000_000) // pause a little bit every million bytes. Otherwise the UI is impossible...
                        {
                            await ui.OnStreamTotalProgressAsync((ulong)totalRead);
                            await Task.Delay(50);
                            tempTotalRead = 0;
                        }
                        if (ct.IsCancellationRequested)
                        {
                            keepGoing = false;
                        }
                    }
                }
                catch (Exception readex)
                {
                    ; // all of the actual data-reading exceptions.
                    await ui.LogAsync($"Read error: {readex.Message}");
                }
                // Must flush the contents
                outstream.Flush();
                outstream.Close();
            }
            await ui.OnStreamTotalProgressAsync((ulong)totalRead);
            await ui.OnStreamCompleteAsync();
            return retval;
        }
    }
}
