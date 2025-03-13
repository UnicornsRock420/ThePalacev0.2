using ThePalace.Client.Desktop.Enums;

namespace ThePalace.Client.Desktop.Interfaces;

public interface ILayerScreen : IDisposable
{
    bool Visible { get; set; }
    float Opacity { get; set; }
    Bitmap Image { get; }

    IDesktopSessionState SessionState { get; }
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