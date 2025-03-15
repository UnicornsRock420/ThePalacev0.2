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
    
    string? Title { get; set; }
    string? Type { get; set; }
    ApiBinding? Binding { get; set; }
    bool Checked { get; set; }
    bool Enabled { get; }

    string? HoverIcon { get; set; }
    Bitmap[]? HoverFrames { get; set; }
}