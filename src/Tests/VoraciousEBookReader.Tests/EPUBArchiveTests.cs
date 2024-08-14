using System.Data;

using VoraciousEBookReader.EPUB.ViewModel;

namespace VoraciousEBookReader.Tests;

[TestClass]
public partial class EPUBArchiveTests
{
    // A sample EPUB file
    private const string epubfile = @"C:\\Users\\drcarver\\Desktop\\VoraciousEBookReader\\src\\PreinstalledBooks\\Meditations_by_Marcus_Aurelius_Emperor.epub";

    [TestMethod]
    public void EPUBArchiveCreateFromFilePathTests()
    {
        EPUBArchiveViewModel epub = new EPUBArchiveViewModel(epubfile);
        Assert.IsTrue(epub != null);
        epub.ArchiveStream.Close();
        epub = null;
    }

    [TestMethod]
    public void EPUBArchiveReadEntryTests()
    {
        EPUBArchiveViewModel epub = new EPUBArchiveViewModel(epubfile);
        Assert.IsTrue(epub != null);
        var fileentry = epub.FindEntry("mimetype");
        epub.ArchiveStream.Close();
        epub = null;
    }
}

// mimetype