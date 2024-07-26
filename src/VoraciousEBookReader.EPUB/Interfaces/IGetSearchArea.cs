using System.Collections.Generic;

namespace VoraciousEBookReader.EPUB.Interfaces
{
    public interface IGetSearchArea
    {
        IList<string> GetSearchArea(string inputArea);
    }
}
