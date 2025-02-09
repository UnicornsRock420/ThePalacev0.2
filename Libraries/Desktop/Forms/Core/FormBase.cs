using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Forms.Core
{
    public class FormBase : Form
    {
        public FormBase() => _cursor = new Cursor(Cursor.Current.Handle);

        ~FormBase() => base.Dispose(false);

        private readonly Cursor _cursor;
        public Cursor Cursor => _cursor;

        public IUISessionState SessionState;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (HotKeyManager.Current.Invoke(SessionState, keyData, this)) return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}