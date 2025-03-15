using System.Collections.Concurrent;
using System.Reflection;
using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Entities.Ribbon;

public abstract class ItemBase : IDisposable, IRibbon<ItemBase>
{
    ~ItemBase()
    {
        Dispose();
    }

    public void Dispose()
    {
        Unload();

        _hoverFrames?.ToList()?.ForEach(f =>
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

        Binding = null;

        GC.SuppressFinalize(this);
    }

    public Guid Id { get; } = Guid.NewGuid();

    public virtual string? Type { get; internal set; } = null;
    public virtual string? Title { get; set; } = null;
    public virtual bool Enabled { get; set; } = false;
    public virtual bool Checked { get; set; } = false;
    public virtual bool Checkable { get; internal set; } = false;

    public virtual ApiBinding? Binding { get; internal set; } = null;

    public virtual string? HoverKey { get; internal set; } = null;

    internal int _hoverFrameIndex = 0;
    internal virtual List<Bitmap>? _hoverFrames { get; set; } = null;
    public virtual IReadOnlyList<Bitmap>? HoverFrames => _hoverFrames.AsReadOnly();

    public void Unload()
    {
        if ((_hoverFrames?.Count ?? 0) > 0)
            foreach (var frame in HoverFrames)
                try
                {
                    frame?.Dispose();
                }
                catch
                {
                }

        switch (this)
        {
            case StandardItem _standardItem:
                try
                {
                    _standardItem.Icon?.Dispose();
                }
                catch
                {
                }

                break;
            case BooleanItem _booleanItem:
                try
                {
                    _booleanItem.OnIcon?.Dispose();
                }
                catch
                {
                }

                try
                {
                    _booleanItem.OffIcon?.Dispose();
                }
                catch
                {
                }

                break;
        }
    }

    public void Load(Assembly assembly, string rootPath)
    {
        Unload();

        if (!string.IsNullOrWhiteSpace(HoverKey))
            using (var hoverImage = GetIcon(assembly, $"{rootPath}.{HoverKey}"))
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

                _hoverFrames = hoverFrames;
            }

        switch (this)
        {
            case StandardItem _standardItem:
            {
                if (!string.IsNullOrWhiteSpace(_standardItem.Key))
                {
                    var path = Path.GetFileNameWithoutExtension($"{rootPath}.{_standardItem.Key}");
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        var icon = GetIcon(assembly, path);
                        _standardItem.Icon = new IconBase(new KeyValuePair<string, Bitmap>(path, icon));
                    }
                }

                break;
            }
            case BooleanItem _booleanItem:
            {
                var path = Path.GetFileNameWithoutExtension($"{rootPath}.{_booleanItem.OnIcon}");
                if (!string.IsNullOrWhiteSpace(path))
                {
                    var icon = GetIcon(assembly, path);
                    _booleanItem.OnIcon = new IconBase(new KeyValuePair<string, Bitmap>(path, icon));
                }

                path = Path.GetFileNameWithoutExtension($"{rootPath}.{_booleanItem.OffIcon}");
                if (!string.IsNullOrWhiteSpace(path))
                {
                    var icon = GetIcon(assembly, path);
                    _booleanItem.OffIcon = new IconBase(new KeyValuePair<string, Bitmap>(path, icon));
                }

                break;
            }
        }
    }

    public Bitmap NextFrame()
    {
        Interlocked.Increment(ref _hoverFrameIndex);

        return HoverFrames[_hoverFrameIndex %= HoverFrames.Count];
    }

    public void ResetFrames()
    {
        _hoverFrameIndex = 0;
    }

    public ToolStripItem ToButton(ConcurrentDictionary<Guid, ItemBase> ribbon, ToolStrip container)
    {
        var nodeType = GetType();
        switch (this)
        {
            case Separator _: return new ToolStripSeparator { AutoSize = false, Height = container.Height };
            default:
                var result = new ToolStripButton();

                //var binding = ApiManager.Current.ApiBindings.GetValue("toolStripDropdownlist_Click");
                //if (binding != null)
                //    item.Click += binding.Binding;

                if (Checkable)
                {
                    var key = this?.Id as Guid?;
                    if (key == null) return null;

                    var this2 = ribbon.GetValue(key.Value);
                    if (this2 != null)
                        result.Checked = !this2.Checked;
                }

                // TODO: History
                var condition = nodeType switch
                {
                    _ => true
                };

                switch (this)
                {
                    case StandardItem _standardItem:
                        result.Image = _standardItem.Icon.Image;
                        break;
                    case BooleanItem _booleanItem:
                        result.Image = _booleanItem.State ? _booleanItem.OnIcon.Image : _booleanItem.OffIcon.Image;
                        break;
                }

                if (result.Image != null)
                {
                    result.Name = nodeType.Name;
                    result.Enabled = condition;
                    result.ToolTipText = Title;

                    return result as ToolStripItem;
                }

                break;
        }

        return null;
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
                t.Namespace.StartsWith("ThePalace") &&
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
        result.HoverKey = instance.HoverKey;
        result._hoverFrames = instance._hoverFrames;
        switch (result)
        {
            case StandardItem _standardItem1 when
                instance is StandardItem _standardItem2:
                _standardItem1.Key = _standardItem2.Key;
                _standardItem1.Icon = _standardItem2.Icon;
                break;
            case BooleanItem _booleanItem1 when
                instance is BooleanItem _booleanItem2:
                _booleanItem1.OnIcon = _booleanItem2.OnIcon;
                _booleanItem1.OnIcon = _booleanItem2.OnIcon;
                _booleanItem1.OffIcon = _booleanItem2.OffIcon;
                _booleanItem1.OffIcon = _booleanItem2.OffIcon;
                break;
        }

        return result;
    }
}