using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Bson;
using SimpleEPUBReader.Database;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleEPUBReader.Controls
{
    public sealed partial class ReviewNoteStatusControl : UserControl
    {
        public ReviewNoteStatusControl()
        {
            this.DataContextChanged += ReviewNoteStatusControl_DataContextChanged;
            this.InitializeComponent();
        }

        private void ReviewNoteStatusControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            // DataContext must be a Books
        }

        /// <summary>
        /// Ensures that Books has a Review item and that it's hooked to the Books via the BookId.
        /// </summary>
        private void EnsureReview()
        {
            if (Books != null && Books.Review == null)
            {
                Books.Review = new UserReview() { BookId = Books.BookId };
            }
        }

        public void SaveData()
        {
            var bookdb = BooksContext.Get();

            if (Books != null)
            {
                EnsureReview();
                Books.Review.Review = uiReview.TextValue;
                Books.Review.Tags = uiTags.Text;
                CommonQueries.BookSaveChanges(bookdb);
            }

            // // // TODO: actually save note.... uiNotes.SaveNote(); //
            uiNoteEditor.SaveNoteIfNeeded(Navigator.NavigateControlId.MainReader, bookdb);

        }

        UserNote PotentialNote = null;
        public void SetupNote(UserNote potentialNote)
        {
            PotentialNote = potentialNote;
        }

        public Books Books { get { return this.DataContext as Books; } set { if (this.DataContext != value) this.DataContext = value; } }

        public void SetBooks(Books Books, string defaultText)
        {
            Books = Books;
            if (Books == null) return; // should never happen.
            EnsureReview();
            if (string.IsNullOrEmpty(Books.Review.Review))
            {
                Books.Review.Review = defaultText;
            }
            uiBookCard.DataContext = Books;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (Books == null) return;

            uiNCatalogViews.Text = Books.NavigationData?.NCatalogViews.ToString() ?? "?";
            uiNSwipeLeft.Text = Books.NavigationData?.NSwipeLeft.ToString() ?? "?";
            uiNSwipeRight.Text = Books.NavigationData?.NSwipeRight.ToString() ?? "?";
            uiNReading.Text = Books.NavigationData?.NReading.ToString() ?? "?";
            uiNSpecificSelection.Text = Books.NavigationData?.NSpecificSelection.ToString() ?? "?";

            uiCurrSpot.Text = Books.NavigationData?.CurrSpot ?? "?";
            uiCurrStatus.Text = Books.NavigationData?.CurrStatus.ToString() ?? "?";

            uiBookId.Text = Books.BookId;
            uiFilePath.Text = Books.DownloadData?.FilePath ?? "?";
            uiFileName.Text = Books.DownloadData?.FileName ?? "?";
            uiFileStatus.Text = Books.DownloadData?.CurrFileStatus.ToString() ?? "?";

            uiTags.Text = Books.Review?.Tags ?? "";
            if (Books.Review != null && Books.Review.NStars > 0) uiNStars.Value = Books.Review.NStars;
            uiReview.SetText(Books.Review?.Review ?? "");

            // And get the status from the navigation data. 
            if (Books.NavigationData != null)
            {
                switch (Books.NavigationData.CurrStatus)
                {
                    case BookNavigationData.UserStatus.Abandoned:
                        uiUserStatusAbandoned.IsChecked = true;
                        break;
                    case BookNavigationData.UserStatus.Done:
                        uiUserStatusDone.IsChecked = true;
                        break;
                    case BookNavigationData.UserStatus.Reading:
                        uiUserStatusReading.IsChecked = true;
                        break;
                }
            }
        }

        private void OnTagChanged(object sender, TextChangedEventArgs e)
        {
            // Actually saved on unload.
        }

        private void OnStarRatingChanged(RatingControl sender, object args)
        {
            var value = sender.Value;
            EnsureReview();
            Books.Review.NStars = value;
            var bookdb = BooksContext.Get();
            CommonQueries.BookSaveChanges(bookdb);
        }

        private void OnUserStatusChecked(object sender, RoutedEventArgs e)
        {
            if (Books.NavigationData != null)
            {
                if (uiUserStatusAbandoned.IsChecked.Value) Books.NavigationData.CurrStatus = BookNavigationData.UserStatus.Abandoned;
                if (uiUserStatusDone.IsChecked.Value) Books.NavigationData.CurrStatus = BookNavigationData.UserStatus.Done;
                if (uiUserStatusReading.IsChecked.Value) Books.NavigationData.CurrStatus = BookNavigationData.UserStatus.Reading;
                var bookdb = BooksContext.Get();
                CommonQueries.BookSaveChanges(bookdb);
            }
        }

        private void OnReviewOrNote(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1) return;
            var tvi = e.AddedItems[0] as TabViewItem;
            if (tvi.Tag as string == "//notes//")
            {
                // Lets use the potential note.
                var ne = tvi.Content as NoteEditor;
                if (ne == null) { App.Error("ERROR: can't edit the note because the menu has the wrong type in it"); return; }
                ne.DataContext = PotentialNote;
            }
        }
    }
}
