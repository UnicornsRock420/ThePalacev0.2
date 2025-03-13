using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Forms.Core;

public class FormBase : Form
{
    ~FormBase()
    {
        base.Dispose();
    }

    public IUISessionState SessionState;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (HotKeyManager.Current.Invoke(SessionState, keyData, this)) return true;

        return base.ProcessCmdKey(ref msg, keyData);
    }
}