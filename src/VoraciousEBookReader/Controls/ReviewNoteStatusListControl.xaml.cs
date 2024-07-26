using SimpleEPUBReader.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace SimpleEPUBReader.Controls
{
    public sealed partial class ReviewNoteStatusListControl : UserControl
    {
        public ObservableCollection<Books> BookList { get; } = new ObservableCollection<Books>();
        public ReviewNoteStatusListControl()
        {
            this.DataContext = this;
            this.InitializeComponent();
        }
        public void SetBookList(IList<Books> list)
        {
            BookList.Clear();
            foreach (var Books in list)
            {
                BookList.Add(Books);
            }
        }


        private void OnNext(object sender, RoutedEventArgs e)
        {

        }
        private void OnPrev(object sender, RoutedEventArgs e)
        {

        }

        private void OnSave(object sender, RoutedEventArgs e)
        {

        }

        private void OnBooksGotFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
