using ThePalace.Common.Desktop.Entities.Core;

namespace ThePalace.Common.Desktop.Entities.UI;

public class HotKeyBinding
{
    public ApiBinding ApiBinding { get; set; } = null;
    public object[] Values { get; set; } = null;
}