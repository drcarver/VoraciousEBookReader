using System.Collections.ObjectModel;

using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.EBookReader.Interface;

public interface IListViewModelBase
{
    /// <summary>
    /// The view model base entry
    /// </summary>
    ObservableCollection<IBaseViewModel> ListViewModelEntries { get; }

    /// <summary>
    /// The selected item
    /// </summary>
    ObservableCollection<IBaseViewModel> SelectedItems { get; set; }
}
