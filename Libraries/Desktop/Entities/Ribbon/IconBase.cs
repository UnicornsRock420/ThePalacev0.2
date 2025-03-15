using System.Collections.Concurrent;
using System.Reflection;
using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Entities.Ribbon;

public class IconBase : IDisposable
{
    private IconBase()
    {
    }

    public IconBase(params KeyValuePair<string, Bitmap>[] hoverFrames)
    {
        _hoverFrames = new(hoverFrames);
    }

    ~IconBase()
    {
        Dispose();
    }

    public void Dispose()
    {
        Unload();

        _hoverFrames.Values?.ToList()?.ForEach(f =>
        {
            try
            {
                f?.Dispose();
            }
            catch
            {
            }
        });
        _hoverFrames = null;

        GC.SuppressFinalize(this);
    }

    public Guid Id { get; } = Guid.NewGuid();

    internal int _hoverFrameIndex = 0;
    internal virtual ConcurrentDictionary<string, Bitmap>? _hoverFrames { get; set; } = null;
    public virtual IReadOnlyDictionary<string, Bitmap>? HoverFrames => _hoverFrames.AsReadOnly();

    public void Unload()
    {
        if ((_hoverFrames?.Count ?? 0) > 0)
            foreach (var frame in _hoverFrames.Values.ToList())
                try
                {
                    frame?.Dispose();
                }
                catch
                {
                }

        _hoverFrames?.Clear();
        _hoverFrames = null;
    }

    public virtual Bitmap NextFrame()
    {
        Interlocked.Increment(ref _hoverFrameIndex);

        return _hoverFrames.Values
            .ToArray()
            .Skip(_hoverFrameIndex %= _hoverFrames.Count)
            .First();
    }

    public virtual Bitmap Image => NextFrame();

    public void ResetFrames()
    {
        _hoverFrameIndex = 0;
    }

    internal static Bitmap GetIcon(Assembly assembly, string resourcePath)
    {
        if (string.IsNullOrWhiteSpace(resourcePath)) return null;

        try
        {
            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null) return null;

            return new Bitmap(stream);
        }
        catch
        {
        }

        return null;
    }
}