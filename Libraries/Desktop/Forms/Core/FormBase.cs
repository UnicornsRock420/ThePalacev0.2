using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Forms.Core;

public class FormBase : Form
{
    ~FormBase()
    {
        base.Dispose();
    }

    public IUISessionState<IDesktopApp> SessionState;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        return HotKeyManager.Current.Invoke(SessionState, keyData, this) ||
               base.ProcessCmdKey(ref msg, keyData);
    }
}