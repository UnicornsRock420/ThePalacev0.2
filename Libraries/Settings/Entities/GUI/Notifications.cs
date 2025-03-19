using Lib.Settings.Attributes;
using Lib.Settings.Entities.Generic;

namespace Lib.Settings.Entities.GUI;

[XPath("$.GUI:{}")]
public class Notifications
{
    public clsEnabled Toast  { get; set; }
}