using System.Threading.Tasks;

namespace VoraciousEBookReader.Gutenberg
{
    public class NullIndexReader : IndexReader
    {
        public void BookEnd(BookStatus status, Books book)
        {
        }

        public CoreDispatcher GetDispatcher()
        {
            return null;
        }

        public async Task LogAsync(string str)
        {
            await Task.Delay(0);
        }


        public async Task OnStreamCompleteAsync()
        {
            await Task.Delay(0);
        }

        public async Task OnStreamProgressAsync(uint bytesRead)
        {
            await Task.Delay(0);
        }

        public async Task OnStreamTotalProgressAsync(ulong bytesRead)
        {
            await Task.Delay(0);
        }
        public async Task OnAddNewBook(Books Books)
        {
            await Task.Delay(0);
        }

        public void SetFileSize(ulong size)
        {
        }

        public async Task OnTotalBooks(int nbooks)
        {
            await Task.Delay(0);
        }

        public async Task OnReadComplete(int nBooksTotal, int nNewBooks)
        {
            await Task.Delay(0);
        }

    }
}
