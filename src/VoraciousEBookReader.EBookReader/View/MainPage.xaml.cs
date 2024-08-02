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
}

