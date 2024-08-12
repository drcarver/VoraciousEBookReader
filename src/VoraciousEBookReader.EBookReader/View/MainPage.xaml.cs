using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.EBookReader;

public partial class MainPage : ContentPage
{
    public MainPage(IMainPageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    /// <summary>
    /// Row height
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void dataGrid_QueryRowHeight(object sender, Syncfusion.Maui.DataGrid.DataGridQueryRowHeightEventArgs e)
    {
        if (e.GetIntrinsicRowHeight(0, false) > 48)
        {
            dataGrid.RowHeight = e.GetIntrinsicRowHeight(e.RowIndex, false);
        }
        else
        {
            dataGrid.RowHeight = e.GetIntrinsicRowHeight(e.RowIndex, false) - 24;
        }
    }
}

