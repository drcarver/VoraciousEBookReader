using VoraciousEBookReader.EBookReader.Interface;

namespace VoraciousEBookReader.EBookReader.View;

public partial class WebViewPage : ContentPage
{
	public WebViewPage(IWebViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}