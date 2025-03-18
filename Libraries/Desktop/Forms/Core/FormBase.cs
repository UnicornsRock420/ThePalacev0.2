using Lib.Common.Desktop.Factories;
using Lib.Common.Desktop.Interfaces;

namespace Lib.Common.Desktop.Forms.Core;

public class FormBase : Form
{
    ~FormBase()
    {
        base.Dispose();
    }

    public IUISessionState SessionState;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        return HotKeyManager.Current.Invoke(SessionState, keyData, this) ||
               base.ProcessCmdKey(ref msg, keyData);
    }
}