﻿using Microsoft.Toolkit.Uwp.Helpers;
using SimpleEpubReader.Database;
using SimpleEpubReader.Searching;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleEpubReader.Controls
{
    public sealed partial class EBookReaderPickAndSend : UserControl
    {
        public ObservableCollection<HelperBookDataWithSelected> Books { get; } = new ObservableCollection<HelperBookDataWithSelected>();
        public EBookReaderPickAndSend()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.Loaded += EBookReaderPickAndDownload_Loaded;
        }

        private async void EBookReaderPickAndDownload_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = (await EBookFolder.GetFolderSilentAsync()).Folder;
            if (folder != null)
            {
                uiUserSaveFolderNote.Visibility = Visibility.Visible;
                uiSaveFolderName.Text = folder.Path;
            }
        }

        private IList<BookData> GetSelectedBooks()
        {
            var retval = new List<BookData>();
            foreach (var book in Books)
            {
                if (book.IsSelected)
                {
                    retval.Add(book.RawBook);
                }
            }
            return retval;
        }

        private async Task UpdateList()
        { 
            // Get list of currently-reading books

            var startTime = DateTime.Now;
            var search = "";
            var searchType = uiShowDownloaded.IsChecked.Value ? "Downloaded" : "Reading";
            var sortBy = "title";
            var language = "en";

            List<BookData> resultList = null;
            bool andMore = false;

            Books.Clear();

            var searchTask = Task.Run(() => {
                var bookdb = BookDataContext.Get();
                lock (bookdb)
                {
                    resultList = AllBookSearch.SearchInternal(bookdb, searchType, search, language, sortBy, out andMore);
                }
            });
            await searchTask;

            // Finally add to the output
            foreach (var book in resultList)
            {
                Books.Add(new HelperBookDataWithSelected(book));
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as ContentDialog;
            parent.Hide();
        }

        private async void OnShowChange(object sender, RoutedEventArgs e)
        {
            await UpdateList();
        }

        private void OnSelectAllCheck(object sender, RoutedEventArgs e)
        {
            CheckAll(true);
        }

        private void OnSelectAllUnchecked(object sender, RoutedEventArgs e)
        {
            CheckAll(false);
        }

        private void CheckAll(bool newCheck)
        {
            foreach (var book in Books)
            {
                book.IsSelected = newCheck;
            }
        }

        private async void OnSend(object sender, RoutedEventArgs e)
        {
            int nok = 0;
            int nfail = 0;

            var bookdb = BookDataContext.Get();
            var selectedBooks = GetSelectedBooks();
            StorageFolder folder = (await EBookFolder.GetFolderSilentAsync()).Folder;
            if (folder == null)
            {
                folder = await EBookFolder.PickFolderAsync();
            }
            EbookReaderProgressControl progress = null;
            if (folder != null)
            {
                progress = new EbookReaderProgressControl();
                uiAlternateContent.Visibility = Visibility.Visible;
                uiAlternateContent.Children.Clear();
                uiAlternateContent.Children.Add(progress);
                progress.SetNBooks(selectedBooks.Count);


                foreach (var bookData in selectedBooks)
                {
                    var srcfullname = bookData.DownloadData.FullFilePath;
                    var fname = bookData.DownloadData.FileName;
                    try
                    {
                        var src = await StorageFile.GetFileFromPathAsync(srcfullname);
                        var exists = await folder.FileExistsAsync(fname);
                        if (!exists)
                        {
                            progress.SetCurrentBook(bookData.Title);
                            Logger.Log($"SEND: sending {fname}");
                            await src.CopyAsync(folder, fname, NameCollisionOption.FailIfExists);
                        }
                        else
                        {
                            progress.AddLog($"No need to send {fname}");
                            Logger.Log($"SEND: no need to send {fname}\n");
                            ; //NOTE: possibly in the future I'll do something useful here -- like offer to 
                            // re-copy the file, or verify that it's at least the same size or something.
                        }
                        var nd = CommonQueries.BookNavigationDataEnsure(bookdb, bookData);
                        nd.CurrStatus = BookNavigationData.UserStatus.CopiedToEBookReader;
                        nok++;
                    }
                    catch (Exception ex)
                    {
                        nfail++;
                        progress.AddLog($"ERROR: exception when sending {fname} message {ex.Message}\n");
                        Logger.Log($"ERROR: COPY: exception when copying {fname} message {ex.Message}");
                    }
                }
            }


            Logger.Log($"COPY: OK={nok} FAIL={nfail}\n");

            if (nok > 0)
            {
                CommonQueries.BookSaveChanges(bookdb);
            }
            if (progress != null)
            {
                progress.AddLog($"Book send complete OK={nok} FAIL={nfail}");
                await Task.Delay(5_000); // wait so the user can see something happened.
            }

            // End by closing the dialog box.
            var parent = this.Parent as ContentDialog;
            parent.Hide();
        }
    }
}
