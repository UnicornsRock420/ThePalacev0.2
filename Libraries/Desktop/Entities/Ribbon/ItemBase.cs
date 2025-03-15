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

        Binding = null;

        GC.SuppressFinalize(this);
    }

    private static readonly Type CONST_TYPE_IRibbon = typeof(IRibbon);

    public Guid Id { get; } = Guid.NewGuid();
    public virtual ApiBinding? Binding { get; internal set; } = null;

    public virtual string? Title { get; set; } = null;
    public virtual string? Style { get; set; } = null;
    public virtual bool Enabled { get; set; } = false;
    public virtual bool Checked { get; set; } = false;
    public virtual bool Checkable { get; set; } = false;

    public void Unload()
    {
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
                    _booleanItem.OnHoverIcon?.Dispose();
                }
                catch
                {
                }

                try
                {
                    _booleanItem.OffHoverIcon?.Dispose();
                }
                catch
                {
                }

                break;
        }
    }

    public void Load(Assembly assembly, string xPath)
    {
        Unload();

        switch (this)
        {
            case StandardItem _standardItem:
            {
                _standardItem.Key = Path.GetFileNameWithoutExtension(xPath);
                if (!string.IsNullOrWhiteSpace(_standardItem.Key))
                {
                    _standardItem.Icon._hoverFrames = GetIcon(assembly, $"{xPath}.{_standardItem.Key}");
                }

                break;
            }
            case BooleanItem _booleanItem:
            {
                _booleanItem.OnHoverKey = Path.GetFileNameWithoutExtension(xPath);
                if (!string.IsNullOrWhiteSpace(_booleanItem.OnHoverKey))
                {
                    _booleanItem.OnHoverIcon._hoverFrames = GetIcon(assembly, $"{xPath}.{_booleanItem.OnHoverKey}");
                }

                _booleanItem.OffHoverKey = Path.GetFileNameWithoutExtension(xPath);
                if (!string.IsNullOrWhiteSpace(_booleanItem.OffHoverKey))
                {
                    _booleanItem.OffHoverIcon._hoverFrames = GetIcon(assembly, $"{xPath}.{_booleanItem.OffHoverKey}");
                }

                break;
            }
        }
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
                        result.Image = _booleanItem.State ? _booleanItem.OnHoverIcon.Image : _booleanItem.OffHoverIcon.Image;
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

    internal static ConcurrentDictionary<string, Bitmap>? GetIcon(Assembly assembly, string resourcePath)
    {
        if (string.IsNullOrWhiteSpace(resourcePath)) return null;

        var hoverFrames = new Dictionary<string, Bitmap>();
        var hoverImage = (Bitmap?)null;

        try
        {
            var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null) return null;

            hoverImage = new Bitmap(stream);
            if (hoverImage == null) return null;

            var length = hoverImage.Height;
            var count = hoverImage.Width / length;

            for (var j = 0; j < count; j++)
            {
                var hoverFrame = new Bitmap(length, length);
                if (hoverFrame == null) throw new OutOfMemoryException(nameof(ItemBase) + "." + nameof(GetIcon) + "." + nameof(hoverFrame));

                hoverFrame.MakeTransparent(Color.Transparent);
                using (var g = Graphics.FromImage(hoverFrame))
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

                hoverFrames.Add(Path.GetFileName(resourcePath), hoverFrame);
            }

            return new(hoverFrames);
        }
        finally
        {
            hoverImage?.Dispose();

            hoverImage = null;
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
                t.GetInterfaces().Contains(CONST_TYPE_IRibbon) &&
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

    public static ItemBase Instance(string nodeType, string style)
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
                if (style == "ddl")
                {
                    result.Style = style;
                    break;
                }

                goto default;
            default:
                result.Style = "btn";
                break;
        }

        return result;
    }

    public static ItemBase Clone(ItemBase instance)
    {
        return Clone(instance, instance.Style);
    }

    public static ItemBase Clone(ItemBase instance, string style)
    {
        var result = Instance(instance.GetType().Name, style);
        result.Title = instance.Title;
        switch (result)
        {
            case StandardItem _standardItem1 when
                instance is StandardItem _standardItem2:
                _standardItem1.Key = _standardItem2.Key;
                _standardItem1.Icon = _standardItem2.Icon;
                break;
            case BooleanItem _booleanItem1 when
                instance is BooleanItem _booleanItem2:
                _booleanItem1.OnHoverKey = _booleanItem2.OnHoverKey;
                _booleanItem1.OnHoverIcon = _booleanItem2.OnHoverIcon;
                _booleanItem1.OffHoverKey = _booleanItem2.OffHoverKey;
                _booleanItem1.OffHoverIcon = _booleanItem2.OffHoverIcon;
                break;
        }

        return result;
    }
}