using CommunityToolkit.Mvvm.ComponentModel;

using VoraciousEBookReader.Gutenberg.Interface;

namespace VoraciousEBookReader.Gutenberg.ViewModel;

public partial class BaseViewModel : ObservableObject, IBaseViewModel
{
    /// <summary>
    /// The page title for the view model
    /// </summary>
    [ObservableProperty]
    private string pageTitle = string.Empty;
}
