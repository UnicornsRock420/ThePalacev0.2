using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Core.Entities.System;

namespace ThePalace.Client.Desktop.Entities
{
    public sealed class ScreenLayer : Disposable
    {
        public ScreenLayers Type { get; private set; }
        public Type ResourceType { get; set; }

        public int Width => Image?.Width ?? 0;
        public int Height => Image?.Height ?? 0;
        public Bitmap Image { get; set; } = null;
        public float Opacity { get; set; } = 1.0F;
        public bool Visible { get; set; } = true;

        public ScreenLayer(ScreenLayers type)
        {
            Type = type;
        }
        ~ScreenLayer() =>
            this.Dispose(false);

        public override void Dispose()
        {
            if (this.IsDisposed) return;

            base.Dispose();

            Unload();
        }

        public void Unload()
        {
            try { Image?.Dispose(); Image = null; } catch { }
        }

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

        public void Load(IUISessionState sessionState, LayerLoadingTypes type, string srcPath, int width = 0, int height = 0)
        {
            if (srcPath == null) throw new ArgumentNullException(nameof(srcPath));

            lock (this)
            {
                var backgroundImage = null as Bitmap;

                switch (type)
                {
                    case LayerLoadingTypes.Filesystem:
                        if (File.Exists(srcPath))
                            try { backgroundImage = new Bitmap(srcPath); } catch { }

                        break;
                    case LayerLoadingTypes.Resource:
                        using (var stream = ResourceType
                            ?.Assembly
                            ?.GetManifestResourceStream(srcPath))
                        {
                            if (stream == null) return;

                            try { backgroundImage = new Bitmap(stream); } catch { }
                        }

                        break;
                }
                if (backgroundImage == null) return;

                Unload();

                Image = backgroundImage;
                Image.Tag = Path.GetFileName(srcPath);

                if (Type == ScreenLayers.Base)
                {
                    //sessionState.ScreenWidth = backgroundImage.Width;
                    //sessionState.ScreenHeight = backgroundImage.Height;
                }
            }
        }
    }
}
