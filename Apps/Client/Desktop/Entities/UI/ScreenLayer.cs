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
    public Bitmap Image { get; set; }

    public ScreenLayerTypes LayerLayerType { get; }
    public int Width => Image?.Width ?? 0;
    public int Height => Image?.Height ?? 0;

    public Graphics Initialize(int width, int height)
    {
        if (Image != null &&
            (Image.Width != width ||
             Image.Height != height))
            Unload();

        if (Image == null)
        {
            Image = new Bitmap(width, height);
            Image.MakeTransparent(Color.Transparent);
        }

        var g = Graphics.FromImage(Image);
        g.Clear(Color.Transparent);

        return g;
    }

    public void Load(
        IDesktopSessionState sessionState,
        LayerLoadingTypes layerType,
        string srcPath,
        int? width = null,
        int? height = null)
    {
        ArgumentNullException.ThrowIfNull(srcPath, nameof(srcPath));

        using (var @lock = LockContext.GetLock(this))
        {
            var backgroundImage = null as Bitmap;

            try
            {
                switch (layerType)
                {
                    case LayerLoadingTypes.Filesystem:
                        if (File.Exists(srcPath))
                            backgroundImage = new Bitmap(srcPath);

                        break;
                    case LayerLoadingTypes.Resource:
                        using (var stream = ResourceType
                                   ?.Assembly
                                   ?.GetManifestResourceStream(srcPath))
                        {
                            if (stream == null) return;

                            backgroundImage = new Bitmap(stream);
                        }

                        break;
                }
            }
            catch
            {
            }

            if (backgroundImage == null) return;

            Unload();

            Image = backgroundImage;
            Image.Tag = Path.GetFileName(srcPath);

            if (LayerLayerType != ScreenLayerTypes.Base &&
                (!width.HasValue || !height.HasValue)) return;

            sessionState.ScreenWidth = width ?? backgroundImage.Width;
            sessionState.ScreenHeight = height ?? backgroundImage.Height;
        }
    }

    public void Unload()
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