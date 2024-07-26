using VoraciousEBookReader.EBookReader;

namespace VoraciousEBookReader.MAUI.EbookReader
{
    public partial class Navigator
    {
        public interface INavigateTo
        {
            /// <summary>
            /// Individual controls implement this; when the user navigates (e.g., from the chapter list),
            /// each control learns about it because their implementation of NavigateTo is called.
            /// </summary>
            /// <param name="sourceId"></param>
            /// <param name="location"></param>
            void NavigateTo(NavigateControlId sourceId, BookLocation location);
        }
    }
}
