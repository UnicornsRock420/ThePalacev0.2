using ThePalace.Common.Desktop.Entities.Core;

namespace ThePalace.Common.Desktop.Entities.UI;

public partial class HotKeyBinding
{
    public ApiBinding Binding { get; set; } = null;
    public object[] Values { get; set; } = null;
}