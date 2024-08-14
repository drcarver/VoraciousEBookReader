// Ignore Spelling: EPUB

using System;
using System.IO;
using System.IO.Compression;

using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.EPUB.Enum;
using VoraciousEBookReader.EPUB.Extensions;

namespace VoraciousEBookReader.EPUB.ViewModel
{
    /// <summary>
    /// The archive for the EPUB
    /// </summary>
    public partial class EPUBArchiveViewModel : ObservableObject
    {
        /// <summary>
        /// The zip archive for the EPUB
        /// </summary>
        [ObservableProperty]
        private ZipArchive? archive;

        /// <summary>
        /// The archiveStream
        /// </summary>
        [ObservableProperty]
        private Stream archiveStream;

        /// <summary>
        /// Open a EPUB from a file
        /// </summary>
        /// <param name="filePath">The path to the EPUB file</param>
        /// <exception cref="ArgumentNullException">Raised if the filePath is null</exception>
        /// <exception cref="FileNotFoundException">Raise if the file is not found in the filePath</exception>
        public EPUBArchiveViewModel(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Specified EPUB file not found.", filePath);
            }
            ArchiveStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            Archive = Open(false);
        }

        /// <summary>
        /// Open the archive from the file stream
        /// </summary>
        /// <param name="stream">The stream containing the EPUB</param>
        /// <param name="leaveOpen">Do we leave the stream open</param>
        /// <returns>Returns the ZipArchive from the stream</returns>
        private ZipArchive Open(bool leaveOpen)
        {
            Archive = new ZipArchive(ArchiveStream, ZipArchiveMode.Read, leaveOpen, Constants.DefaultEncoding);
            return Archive;        
        }

        /// <summary>
        /// Returns an archive entry or null if not found.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ZipArchiveEntry FindEntry(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            return Archive?.TryGetEntryImproved(path);
        }
    }
}
