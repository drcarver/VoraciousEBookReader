using Microsoft.Maui.Controls.PlatformConfiguration;

namespace VoraciousEBookReader.MAUI.EbookReader
{
    public partial class Navigator
    {
        public interface ISetAppColors
        {
            void SetAppColors(Windows.UI.Color bg, Windows.UI.Color fg); // TODO: also set accent color; needed for overlays
        }
    }
}
