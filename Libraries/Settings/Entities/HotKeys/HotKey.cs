using Lib.Settings.Attributes;

namespace Lib.Settings.Entities.HotKeys;

[XPath("$.HotKeys[]:{}")]
public class HotKey
{
    public List<string> Keys { get; set; }
    public string Binding { get; set; }
}