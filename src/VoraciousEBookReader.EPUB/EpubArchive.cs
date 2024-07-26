using System;
using System.IO;
using System.IO.Compression;

using VoraciousEBookReader.EPUB.Enum;
using VoraciousEBookReader.EPUB.Extensions;

namespace VoraciousEBookReader.EPUB
{
    public class EPUBArchive
    {
        private readonly ZipArchive archive;

        public EPUBArchive(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Specified EPUB file not found.", filePath);
            }

            archive = Open(File.Open(filePath, FileMode.Open, FileAccess.Read), false);
        }

        public EPUBArchive(byte[] EPUBData)
        {
            archive = Open(new MemoryStream(EPUBData), false);
        }

        public EPUBArchive(Stream stream, bool leaveOpen)
        {
            archive = Open(stream, leaveOpen);
        }

        private ZipArchive Open(Stream stream, bool leaveOpen)
        {
            return new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen, Constants.DefaultEncoding);
        }

        /// <summary>
        /// Returns an archive entry or null if not found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ZipArchiveEntry FindEntry(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            return archive.TryGetEntryImproved(path);
        }
    }
}
