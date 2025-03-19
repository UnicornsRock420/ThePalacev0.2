using Lib.Settings.Attributes;

namespace Lib.Settings.Entities.GUI.Connection;

[XPath("$.GUI:Connection:[]")]
public class Addresses : UniqueList<string>
{
}