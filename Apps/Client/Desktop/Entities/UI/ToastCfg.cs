using ThePalace.Common.Interfaces.Threading;

namespace ThePalace.Client.Desktop.Entities.UI;

#if WINDOWS10_0_17763_0_OR_GREATER
public class ToastCfg : ICmd
{
    public DateTimeOffset ExpirationTime { get; set; }
    public IReadOnlyDictionary<string, object> Args { get; set; }
    public IReadOnlyList<string> Text { get; set; } = null;
}
#endif