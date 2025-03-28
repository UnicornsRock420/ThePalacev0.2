﻿using System.Reflection;
using Lib.Common.Desktop.Entities.Core;
using Lib.Common.Desktop.Entities.Ribbon;

namespace Lib.Common.Desktop.Interfaces;

public interface IRibbon : IRibbon<ItemBase>
{
}

public interface IRibbon<TRibbon>
    where TRibbon : ItemBase
{
    ApiBinding? Binding { get; }

    string? Title { get; set; }
    string? Style { get; set; }
    bool Enabled { get; set; }
    bool Checked { get; set; }
    bool Checkable { get; protected set; }

    void Load(Assembly assembly, string xPath);
    void Unload();
}