namespace ThePalace.Client.Desktop.Entities.UI;

#if WINDOWS10_0_17763_0_OR_GREATER
public partial class ToastCfg
{
    public DateTimeOffset ExpirationTime { get; set; }
    public IReadOnlyDictionary<string, object> Args { get; set; }
    public IReadOnlyList<string> Text { get; set; } = null;
}
#endif