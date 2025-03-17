using Lib.Common.Desktop.Interfaces;
using ThePalace.Client.Desktop.Enums;

namespace ThePalace.Client.Desktop.Interfaces;

public interface ILayerScreen : IDisposable
{
    bool Visible { get; set; }
    bool Enabled { get; set; }

    float Opacity { get; set; }
    Bitmap Image { get; }

    IClientDesktopSessionState<IDesktopApp> SessionState { get; }
    LayerScreenTypes Type { get; }
    int Width { get; }
    int Height { get; }

    Graphics Clear(int width, int height);

    void Load(
        LayerSourceTypes srcType,
        string xPath,
        int? width = null,
        int? height = null);

    void Unload();
}