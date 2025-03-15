using ThePalace.Common.Desktop.Entities.Core;
using ThePalace.Common.Desktop.Entities.Ribbon;

namespace ThePalace.Common.Desktop.Interfaces;

public interface IRibbon : IRibbon<ItemBase>
{
}

public interface IRibbon<TRibbon>
    where TRibbon : ItemBase
{
    Guid Id { get; }

    string? Type { get; }
    string? Title { get; set; }
    bool Enabled { get; set; }
    bool Checked { get; set; }
    bool Checkable { get; }

    ApiBinding? Binding { get; }

    string? HoverKey { get; }

    IReadOnlyList<Bitmap>? HoverFrames { get; }
}