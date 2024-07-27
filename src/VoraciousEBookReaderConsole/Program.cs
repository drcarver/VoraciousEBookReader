using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;

using Microsoft.Extensions.Logging;

using VoraciousEBookReader.Gutenberg;
using VoraciousEBookReader.Gutenberg.Model;

namespace VoraciousEBookReaderConsole;

internal class Program
{
    private const string PGCATALOG = "GutenbergCatalog.json";

    static async Task Main(string[] args)
    {
        string fPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\VoraciousEBookReader\\{PGCATALOG}";
        var fileInfo = new FileInfo(fPath);
        Directory.CreateDirectory(fileInfo?.DirectoryName);
        if (!File.Exists(fPath + ".gz"))
        {
            var sw = new Stopwatch();
            var catalog = new Catalog(new LoggerFactory(), new HttpClientFactory());
            sw.Start();
            await catalog.GetLatestCatalog();
            sw.Stop();
        }

        // now read it back
        var sw3 = new Stopwatch();
        sw3.Start();
        var list = new List<CatalogEntry>();
        using (FileStream compressedFileStream = File.Open(fPath + ".gz", FileMode.Open))
        {
            using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                list = JsonSerializer.Deserialize<List<CatalogEntry>>(decompressionStream);
            }
        }
        sw3.Stop();
        //File.Delete(fPath + ".gz");
        Console.WriteLine("Hello, World!");
    }
}

//public class Program
//{
//    private static string directoryPath = @".\temp";
//    public static void Main()
//    {
//        DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
//        Compress(directorySelected);

//        foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
//        {
//            Decompress(fileToDecompress);
//        }
//    }

//    public static void Compress(DirectoryInfo directorySelected)
//    {
//        foreach (FileInfo fileToCompress in directorySelected.GetFiles())
//        {
//            using (FileStream originalFileStream = fileToCompress.OpenRead())
//            {
//                if ((File.GetAttributes(fileToCompress.FullName) &
//                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
//                {
//                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
//                    {
//                        using (GZipStream compressionStream = new GZipStream(compressedFileStream,
//                           CompressionMode.Compress))
//                        {
//                            originalFileStream.CopyTo(compressionStream);
//                        }
//                    }
//                    FileInfo info = new FileInfo(directoryPath + Path.DirectorySeparatorChar + fileToCompress.Name + ".gz");
//                    Console.WriteLine($"Compressed {fileToCompress.Name} from {fileToCompress.Length.ToString()} to {info.Length.ToString()} bytes.");
//                }
//            }
//        }
//    }

//    public static void Decompress(FileInfo fileToDecompress)
//    {
//        using (FileStream originalFileStream = fileToDecompress.OpenRead())
//        {
//            string currentFileName = fileToDecompress.FullName;
//            string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

//            using (FileStream decompressedFileStream = File.Create(newFileName))
//            {
//                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
//                {
//                    decompressionStream.CopyTo(decompressedFileStream);
//                    Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
//                }
//            }
//        }
//    }
//}