namespace VoraciousEBookReader.EBookReader;

public partial class App : Application
{
    /// <summary>
    /// The App directory
    /// </summary>
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
