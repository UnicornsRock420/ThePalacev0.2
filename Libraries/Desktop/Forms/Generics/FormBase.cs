using Lib.Common.Desktop.Interfaces;
using Lib.Common.Desktop.Singletons;

namespace Lib.Common.Desktop.Forms.Generics;

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