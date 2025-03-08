using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Forms.Core;

public class FormDialog : FormBase, IFormDialog
{
    private const int WM_PARENTNOTIFY = 0x0210;

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_PARENTNOTIFY &&
            !Focused) Activate();
        base.WndProc(ref m);
    }
}