using System.Collections;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Common.Factories.Core;

namespace ThePalace.Client.Desktop.Entities.UI;

public class LayerScreen : Disposable, ILayerScreen
{
    private LayerScreen()
    {
    }

    public LayerScreen(
        IClientDesktopSessionState<IDesktopApp> sessionState,
        LayerScreenTypes type)
    {
        SessionState = sessionState;
        Type = type;
    }

    ~LayerScreen()
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

    public bool Visible { get; set; } = true;
    public bool Enabled { get; set; } = true;

    public float Opacity { get; set; } = 1.0F;
    public Bitmap Image { get; protected set; }

    public IClientDesktopSessionState<IDesktopApp> SessionState { get; }
    public LayerScreenTypes Type { get; }
    public int Width => Image?.Width ?? 0;
    public int Height => Image?.Height ?? 0;

    public void Load(
        LayerSourceTypes srcType,
        string xPath,
        int? width = null,
        int? height = null)
    {
        if (string.IsNullOrWhiteSpace(xPath)) throw new ArgumentNullException(nameof(xPath));

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
                    using (var stream = AppDomain.CurrentDomain
                               .GetAssemblies()
                               .Where(a => a.FullName.StartsWith("ThePalace.Media"))
                               .Where(a =>
                               {
                                   try
                                   {
                                       using (var stream = a.GetManifestResourceStream(xPath))
                                       {
                                           return stream != null;
                                       }
                                   }
                                   catch
                                   {
                                   }

                                   return false;
                               })
                               .Select(a => a.GetManifestResourceStream(xPath))
                               .FirstOrDefault())
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

        if (Type != LayerScreenTypes.Base) return;

        SessionState.ScreenWidth = width ?? backgroundImage.Width;
        SessionState.ScreenHeight = height ?? backgroundImage.Height;
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