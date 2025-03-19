using Lib.Settings.Attributes;

namespace Lib.Settings.Entities.Scripting;

[XPath("$.:{}")]
public class Scripting
{
    public Iptscrae Iptscrae  { get; set; }
    public JavaScript JavaScript  { get; set; }
    public Python Python  { get; set; }
}