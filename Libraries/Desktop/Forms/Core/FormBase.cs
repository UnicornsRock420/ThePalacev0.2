﻿using ThePalace.Common.Desktop.Factories;
using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Forms.Core;

public class FormBase : Form
{
    public IUISessionState SessionState;

    ~FormBase()
    {
        base.Dispose();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (HotKeyManager.Current.Invoke(SessionState, keyData, this)) return true;

        return base.ProcessCmdKey(ref msg, keyData);
    }
}