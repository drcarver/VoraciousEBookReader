using VoraciousEBookReader.EBookReader;

namespace VoraciousEBookReader.MAUI.EbookReader
{
    public partial class Navigator
    {
        static Navigator NavigatorSingleton = new Navigator();

        /// <summary>
        /// Gets the one true navigatator
        /// </summary>
        /// <returns></returns>
        public static Navigator Get()
        {
            return NavigatorSingleton;
        }
        /// <summary>
        /// The bookhandler is the class+object that knows all the details of the book
        /// </summary>
        public BookHandler MainBookHandler { internal get; set; } = null;

        /// <summary>
        /// Setup routine called just a few times at app startup. Tells the navigator which
        /// displays are which.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="navigateTo"></param>
        public void AddNavigateTo(NavigateControlId id, INavigateTo navigateTo)
        {
            if (NavigateTos.ContainsKey(id))
            {
                NavigateTos[id] = navigateTo;
            }
            else
            {
                NavigateTos.Add(id, navigateTo);
            }
        }

        public void AddSelectTo(NavigateControlId id, ISelectTo selectTo)
        {
            SelectTos.Add(id, selectTo);
        }
        public void RemoveSelectTo(NavigateControlId id)
        {
            if (SelectTos.ContainsKey(id))
            {
                SelectTos.Remove(id);
            }
        }

        public void AddSetAppColor(NavigateControlId id, ISetAppColors setColor)
        {
            AppColors[id] = setColor;
        }

        public void AddSimpleBookHandler(NavigateControlId id, SimpleBookHandler simple)
        {
            SimpleBookHandlers.Add(id, simple);
        }

        public bool DisplayBook(NavigateControlId id, Books Books, BookLocation location = null)
        {
            if (MainBookHandler == null) return false;

            // Is the book actually downloaded?
            if (Books.DownloadData == null || Books.DownloadData.CurrFileStatus != DownloadData.FileStatus.Downloaded)
            {
                // TODO: download the book
                return false;
            }

            MainBookHandler.DisplayBook(Books, location);
            foreach (var item in SimpleBookHandlers)
            {
                // No hairpin selects
                if (item.Key != id)
                {
                    item.Value.DisplayBook(Books, location);
                }
            }

            return true;
        }

        public void SetAppColors(Windows.UI.Color bg, Windows.UI.Color fg)
        {
            foreach (var (item, value) in AppColors)
            {
                value.SetAppColors(bg, fg);
            }
        }

        public void UpdatedNotes(NavigateControlId id)
        {
            foreach (var item in SimpleBookHandlers)
            {
                // No hairpin selects
                if (item.Key != id)
                {
                    if (item.Key == NavigateControlId.NoteListDisplay || item.Key == NavigateControlId.BookSearchDisplay)
                    {
                        item.Value.DisplayBook(null, null); // refreshes the book
                    }
                }
            }
        }

        /// <summary>
        /// Called (often by the main display) when the user selects some book text.
        /// AFAICT, there's no reason for anyone else to call this. The end result is
        /// that e.g. the web search gets the selected text and does a web search.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="selection"></param>
        public void UserSelected(NavigateControlId sourceId, string selection)
        {
            foreach (var (id, control) in SelectTos)
            {
                if (id != sourceId)
                {
                    control.SelectTo(sourceId, selection);
                }
            }

        }
        /// <summary>
        /// Called by any of the displays when the user has picked a place to navigate to.
        /// Is never called automatically. The place is a place inside the already-viewed ebook.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="location"></param>
        public void UserNavigatedTo(NavigateControlId sourceId, BookLocation location)
        {
            Logger.Log($"UserNavigatedTo({location})");
            foreach (var (id, control) in NavigateTos)
            {
                if (id != sourceId)
                {
                    control.NavigateTo(sourceId, location);
                }
            }
        }
        public void UpdateProjectRome(NavigateControlId sourceId, BookLocation location)
        {
            foreach (var (id, control) in NavigateTos)
            {
                if (id != sourceId && id == NavigateControlId.ProjectRome)
                {
                    control.NavigateTo(sourceId, location);
                }
            }
        }

#if NEVER_EVER_DEFINED
        public void UserPickedEbook(NavigateControlId sourceId, string bookId)
        {

        }
#endif

        Dictionary<NavigateControlId, ISetAppColors> AppColors = new Dictionary<NavigateControlId, ISetAppColors>();
        Dictionary<NavigateControlId, INavigateTo> NavigateTos = new Dictionary<NavigateControlId, INavigateTo>();
        Dictionary<NavigateControlId, ISelectTo> SelectTos = new Dictionary<NavigateControlId, ISelectTo>();
        Dictionary<NavigateControlId, SimpleBookHandler> SimpleBookHandlers = new Dictionary<NavigateControlId, SimpleBookHandler>();
    }
}
