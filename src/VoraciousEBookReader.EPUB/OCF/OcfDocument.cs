using System.Collections.Generic;
using System.Linq;

using VoraciousEBookReader.EPUB.Enum;

namespace VoraciousEBookReader.EPUB.OCF
{
    public class OcfDocument
    {
        private OcfRootFile rootFile;

        public IList<OcfRootFile> RootFiles { get; internal set; } = new List<OcfRootFile>();

        public string RootFilePath => rootFile?.FullPath ?? (rootFile = RootFiles.FirstOrDefault(e => e.MediaType == Constants.OcfMediaType))?.FullPath;
    }
}
