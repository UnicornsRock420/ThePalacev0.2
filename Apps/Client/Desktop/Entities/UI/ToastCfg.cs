using Lib.Common.Interfaces.Threading;
using Microsoft.Toolkit.Uwp.Notifications;

namespace ThePalace.Client.Desktop.Entities.UI;

#if WINDOWS10_0_17763_0_OR_GREATER
public class ToastCfg : ICmd
{
    public DateTimeOffset ExpirationTime { get; set; }
    public IReadOnlyDictionary<string, object> Args { get; set; }
    public IReadOnlyList<string> Text { get; set; } = null;

    public static void Dispatch(ToastCfg toastCfg)
    {
        var toast = new ToastContentBuilder();

        foreach (var arg in toastCfg.Args)
        {
            var _type = arg.Value.GetType();
            if (_type == Int32Exts.Types.Int32)
                toast.AddArgument(arg.Key, (int)arg.Value);
            else if (_type == StringExts.Types.String)
                toast.AddArgument(arg.Key, (string)arg.Value);
            else if (_type == DoubleExts.Types.Double)
                toast.AddArgument(arg.Key, (double)arg.Value);
            else if (_type == BooleanExts.Types.Boolean)
                toast.AddArgument(arg.Key, (bool)arg.Value);
            else if (_type == FloatExts.Types.Float)
                toast.AddArgument(arg.Key, (float)arg.Value);
        }

        foreach (var txt in toastCfg.Text)
            toast.AddText(txt);

        toast.Show(t => { t.ExpirationTime = toastCfg.ExpirationTime; });
    }
}
#endif