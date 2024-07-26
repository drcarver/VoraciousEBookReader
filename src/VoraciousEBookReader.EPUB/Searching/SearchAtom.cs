using System;
using System.Collections.Generic;

using VoraciousEBookReader.EPUB.Interfaces;

namespace VoraciousEBookReader.EPUB.Searching
{

    public class SearchAtom : ISearch
    {
        public SearchAtom(string area, string searchFor)
        {
            SearchArea = area;
            SearchFor = searchFor;
        }

        /// <summary>
        /// Example: tag:#notdisney the searcharea is tag. Areas have to match; neither t:#notdisney nor tagtag:#notdisney will mawtch tag:#notdisney
        /// </summary>
        public string SearchArea { get; set; }
        /// <summary>
        /// Example: tag:#notdisney the searchfor is #notdisney
        /// </summary>
        public string SearchFor { get; set; }
        public enum SearchType { StringSearch }
        /// <summary>
        /// For now, there's only one search type
        /// </summary>
        public SearchType Type { get; set; }

        public bool IsNegated { get; set; } = false;
        public void SetIsNegated(bool isNegated)
        {
            IsNegated = isNegated;
        }
        public bool Matches(IGetSearchArea searchObject)
        {
            IList<string> inputList = searchObject.GetSearchArea(SearchArea);
            // might be just one string, like title:apple or might be everything like brown
            int index;
            switch (Type)
            {
                // input="mars attacks" searchfor="MARS" --> found
                // input="" searchFor="MARS" --> not found
                // input="mars attack" searchFor="" --> found
                // input="" searchFor="" --> found
                default:
                case SearchType.StringSearch:
                    {
                        if (string.IsNullOrEmpty(SearchFor))
                        {
                            return true; // must return something...
                        }
                        else
                        {
                            // if any any item matches, return true/false based on isNegated
                            foreach (var item in inputList)
                            {
                                index = item.IndexOf(SearchFor, StringComparison.CurrentCultureIgnoreCase);
                                if (index >= 0)
                                {
                                    return IsNegated ? false : true;
                                }
                            }
                        }
                        // Didn't find it. Return false normally, but true if isnegated.
                        return IsNegated ? true : false;
                    }
            }
        }
        public bool MatchesFlat(string search) // is the giant gnarly per-book string in the index file
        {
            if (IsNegated) return true; // ignore all negated values

            if (string.IsNullOrEmpty(SearchFor))
            {
                return true; // must return something...
            }
            else
            {
                var index = search.IndexOf(SearchFor, StringComparison.CurrentCultureIgnoreCase);
                if (index >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool SearchAreaMatches(string inputArea)
        {
            if (string.IsNullOrEmpty(SearchArea)) return true;
            if (string.Compare(inputArea, SearchArea, true) == 0) return true;
            return false;
        }

        public override string ToString()
        {
            var strength = IsNegated ? "!" : "";
            if (string.IsNullOrEmpty(SearchArea)) return strength + SearchFor;
            return $"{strength}{SearchArea}:{SearchFor}";
        }
    }
}
