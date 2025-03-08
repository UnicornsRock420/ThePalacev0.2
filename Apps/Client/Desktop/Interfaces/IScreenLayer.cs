using ThePalace.Client.Desktop.Enums;

namespace ThePalace.Common.Desktop.Interfaces;

public interface IScreenLayer : IDisposable
{
    Type ResourceType { get; set; }
    bool Visible { get; set; }
    float Opacity { get; set; }
    Bitmap Image { get; set; }

    ScreenLayerTypes LayerLayerType { get; }
    int Width { get; }
    int Height { get; }

    Graphics Initialize(int width, int height);

    void Load(
        IDesktopSessionState sessionState,
        LayerLoadingTypes layerType,
        string srcPath,
        int? width = null,
        int? height = null);

    void Unload();
}