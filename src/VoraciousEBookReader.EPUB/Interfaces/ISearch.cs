namespace VoraciousEBookReader.EPUB.Interfaces
{
    public interface ISearch
    {
        bool Matches(IGetSearchArea searchObject); // Assume that the list always matches the SeachArea 
        // (e.g., we never pass in a title when the search area is a tag)
        void SetIsNegated(bool isNegated);

        bool MatchesFlat(string text);
    }
}
