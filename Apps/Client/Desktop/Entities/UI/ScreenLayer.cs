using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Common.Factories.Core;
using ThePalace.Common.Factories.System.Collections;

namespace ThePalace.Client.Desktop.Entities.UI;

public class ScreenLayer : Disposable, IScreenLayer
{
    private ScreenLayer()
    {
    }

    public ScreenLayer(ScreenLayerTypes layerType)
    {
        LayerLayerType = layerType;
    }

    ~ScreenLayer()
    {
        Dispose();
    }

    public override void Dispose()
    {
        if (IsDisposed) return;

        Unload();

        base.Dispose();

        GC.SuppressFinalize(this);
    }

    public Type ResourceType { get; set; }
    public bool Visible { get; set; } = true;
    public float Opacity { get; set; } = 1.0F;
    public Bitmap Image { get; protected set; }

    public ScreenLayerTypes LayerLayerType { get; }
    public int Width => Image?.Width ?? 0;
    public int Height => Image?.Height ?? 0;

    public void Load(
        LayerSourceTypes srcType,
        string xPath,
        IDesktopSessionState? sessionState = null,
        int? width = null,
        int? height = null)
    {
        ArgumentNullException.ThrowIfNull(xPath, nameof(xPath));

        var backgroundImage = (Bitmap?)null;

        try
        {
            switch (srcType)
            {
                case LayerSourceTypes.Filesystem:
                    if (File.Exists(xPath))
                        backgroundImage = new Bitmap(xPath);

                    break;
                case LayerSourceTypes.Resource:
                    using (var stream = ResourceType
                               ?.Assembly
                               ?.GetManifestResourceStream(xPath))
                    {
                        if (stream == null) return;

                        backgroundImage = new Bitmap(stream);
                    }

                    break;
                default: throw new ArgumentOutOfRangeException(nameof(srcType), srcType, null);
            }
        }
        catch
        {
            backgroundImage = null;
        }

        if (backgroundImage == null) return;

        Unload();

        using (var @lock = LockContext.GetLock(this))
        {
            Image = backgroundImage;
            Image.Tag = Path.GetFileName(xPath);
        }

        if (LayerLayerType != ScreenLayerTypes.Base ||
            sessionState == null) return;

        sessionState.ScreenWidth = width ?? backgroundImage.Width;
        sessionState.ScreenHeight = height ?? backgroundImage.Height;
    }

    public void Unload()
    {
        using (var @lock = LockContext.GetLock(this))
        {
            try
            {
                Image?.Dispose();
                Image = null;
            }
            catch
            {
            }
        }
    }

    public Graphics Clear(int width, int height)
    {
        if (Image?.Width != width ||
            Image?.Height != height)
            Unload();

        Image ??= new Bitmap(width, height);
        Image.MakeTransparent(Color.Transparent);

        var g = Graphics.FromImage(Image);
        g.Clear(Color.Transparent);

        return g;
    }
}