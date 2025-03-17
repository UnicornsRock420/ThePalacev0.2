using Lib.Common.Desktop.Entities.Core;

namespace Lib.Common.Desktop.Entities.UI;

public class HotKeyBinding
{
    public ApiBinding ApiBinding { get; set; } = null;
    public object[] Values { get; set; } = null;
}