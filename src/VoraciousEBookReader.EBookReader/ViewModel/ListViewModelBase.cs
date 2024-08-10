using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Syncfusion.Maui.ListView;

using VoraciousEBookReader.EBookReader.Interface;
using VoraciousEBookReader.Gutenberg.Interface;
using VoraciousEBookReader.Gutenberg.ViewModel;

namespace VoraciousEBookReader.EBookReader.ViewModel;

public partial class ListViewModelBase : BaseViewModel, IListViewModelBase
{
    /// <summary>
    /// The view model base entry
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<IBaseViewModel> listViewModelEntries = [];

    /// <summary>
    /// The selected item
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<IBaseViewModel> selectedItems = [];

    /// <summary>
    /// Called when the selection is changing, this is the command version
    /// </summary>
    /// <param name="eventArgs">The selection changed event arguments</param>
    [RelayCommand]
    private void SelectionItemChanged(ItemSelectionChangedEventArgs eventArgs)
    {
        if (eventArgs.AddedItems.Count > 0)
        {
            foreach (var item in eventArgs.AddedItems)
            {
                var selectedItem = item as IBaseViewModel;
                if  (!SelectedItems.Contains(selectedItem))
                {
                    SelectedItems.Add(selectedItem);
                }
            }

            foreach (var item in eventArgs.RemovedItems)
            {
                var selectedItem = item as IBaseViewModel;
                if (SelectedItems.Contains(selectedItem))
                {
                    SelectedItems.Remove(selectedItem);
                }
            }
        }
    }

    ///<summary>
    ///To display tapped item content
    ///</summary>
    [RelayCommand]
    private void ItemTapped(object eventArgs)
    {
    }
}
