using System.Reflection;
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
    ApiBinding? Binding { get; }

    string? Title { get; set; }
    string? Style { get; set; }
    bool Enabled { get; set; }
    bool Checked { get; set; }
    bool Checkable { get; set; }

    void Load(Assembly assembly, string xPath);
    void Unload();
}