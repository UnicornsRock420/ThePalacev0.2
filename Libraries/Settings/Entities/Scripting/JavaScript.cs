using Lib.Settings.Attributes;
using Lib.Settings.Entities.Generic;

namespace Lib.Settings.Entities.Scripting;

[XPath("$.Scripting:{}")]
public class JavaScript
{
    public clsEnabled Cyborgs { get; set; }
}