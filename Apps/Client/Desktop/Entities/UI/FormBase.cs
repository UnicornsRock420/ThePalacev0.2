﻿using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.UI
{
    public class FormBase : Form
    {
        public IUISessionState SessionState;

        public FormBase() { }
        ~FormBase() =>
            base.Dispose(false);

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (HotKeyManager.Current.Invoke(SessionState, keyData, this))
            //return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}