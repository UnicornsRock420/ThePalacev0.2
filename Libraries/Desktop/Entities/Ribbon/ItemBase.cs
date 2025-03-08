using System.Reflection;
using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Exts.System;

namespace ThePalace.Common.Desktop.Entities.Ribbon;

public abstract class ItemBase : IDisposable
{
    private int _hoverFrameIndex;
    public string Title { get; set; }
    public virtual string Type { get; set; }
    public ApiBinding Binding { get; set; } = null;
    public bool Checked { get; set; } = false;
    public virtual bool Checkable { get; } = false;

    public string HoverIcon { get; set; }
    public Bitmap[] HoverFrames { get; set; }

    public void Dispose()
    {
        Unload();

        GC.SuppressFinalize(this);
    }

    public void Unload()
    {
        if ((HoverFrames?.Length ?? 0) > 0)
            foreach (var frame in HoverFrames)
                try
                {
                    frame?.Dispose();
                }
                catch
                {
                }

        if (this is StandardItem _standardItem)
        {
            try
            {
                _standardItem.Image?.Dispose();
            }
            catch
            {
            }
        }
        else if (this is BooleanItem _booleanItem)
        {
            try
            {
                _booleanItem.OnImage?.Dispose();
            }
            catch
            {
            }

            try
            {
                _booleanItem.OffImage?.Dispose();
            }
            catch
            {
            }
        }
    }

    public void Load(Assembly assembly, string rootPath)
    {
        Unload();

        if (!string.IsNullOrWhiteSpace(HoverIcon))
            using (var hoverImage = GetIcon(assembly, $"{rootPath}.{HoverIcon}"))
            {
                var hoverFrames = new List<Bitmap>();

                var length = hoverImage.Height;
                var count = hoverImage.Width / length;

                for (var j = 0; j < count; j++)
                {
                    var frame = new Bitmap(length, length);
                    frame.MakeTransparent(Color.Transparent);
                    using (var g = Graphics.FromImage(frame))
                    {
                        g.Clear(Color.Transparent);
                        g.DrawImage(
                            hoverImage,
                            new Rectangle(
                                0, 0,
                                length, length),
                            length * j, 0,
                            length, length,
                            GraphicsUnit.Pixel);

                        g.Save();
                    }

                    hoverFrames.Add(frame);
                }

                HoverFrames = hoverFrames.ToArray();
            }

        if (this is StandardItem _standardItem)
        {
            if (!string.IsNullOrWhiteSpace(_standardItem.Icon))
                _standardItem.Image = GetIcon(assembly, $"{rootPath}.{_standardItem.Icon}");
        }
        else if (this is BooleanItem _booleanItem)
        {
            if (!string.IsNullOrWhiteSpace(_booleanItem.OnIcon))
                _booleanItem.OnImage = GetIcon(assembly, $"{rootPath}.{_booleanItem.OnIcon}");

            if (!string.IsNullOrWhiteSpace(_booleanItem.OffIcon))
                _booleanItem.OffImage = GetIcon(assembly, $"{rootPath}.{_booleanItem.OffIcon}");
        }
    }

    public Bitmap NextFrame()
    {
        _hoverFrameIndex %= HoverFrames.Length;

        Interlocked.Increment(ref _hoverFrameIndex);

        return HoverFrames[_hoverFrameIndex % HoverFrames.Length];
    }

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

    public static Type Parse<T>()
    {
        return Parse(typeof(T).Name);
    }

    public static Type Parse(string nodeType)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.Namespace.StartsWith("ThePalace.Client.Desktop.Entities.Ribbon") &&
                t.Name == nodeType)
            .FirstOrDefault();
    }

    public static ItemBase Instance<T>()
    {
        return Instance(typeof(T).Name);
    }

    public static ItemBase Instance(string nodeType)
    {
        return (ItemBase)Parse(nodeType).GetInstance();
    }

    public static ItemBase Instance(string nodeType, string buttonType)
    {
        var result = Instance(nodeType);
        if (result == null) return null;

        switch (nodeType)
        {
            case "GoBack":
            case "GoForward":
            case "UsersList":
            case "RoomsList":
            case "Sounds":
                if (buttonType == "ddl")
                {
                    result.Type = buttonType;
                    break;
                }

                goto default;
            default:
                result.Type = "btn";
                break;
        }

        return result;
    }

    public static ItemBase Clone(ItemBase instance)
    {
        return Clone(instance, instance.Type);
    }

    public static ItemBase Clone(ItemBase instance, string buttonType)
    {
        var result = Instance(instance.GetType().Name, buttonType);
        result.Title = instance.Title;
        result.HoverIcon = instance.HoverIcon;
        result.HoverFrames = instance.HoverFrames;
        if (result is StandardItem _standardItem1 &&
            instance is StandardItem _standardItem2)
        {
            _standardItem1.Icon = _standardItem2.Icon;
            _standardItem1.Image = _standardItem2.Image;
        }
        else if (
            result is BooleanItem _booleanItem1 &&
            instance is BooleanItem _booleanItem2)
        {
            _booleanItem1.OnIcon = _booleanItem2.OnIcon;
            _booleanItem1.OnImage = _booleanItem2.OnImage;
            _booleanItem1.OffIcon = _booleanItem2.OffIcon;
            _booleanItem1.OffImage = _booleanItem2.OffImage;
        }

        return result;
    }
}