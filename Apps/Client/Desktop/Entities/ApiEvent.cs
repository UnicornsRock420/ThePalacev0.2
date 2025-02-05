using System;
using System.Windows.Forms;

namespace ThePalace.Client.Desktop.Entities
{
    public sealed class ApiEvent : EventArgs
    {
        public Keys Keys { get; set; }
        public object Sender { get; set; }
        public object[] HotKeyState { get; set; }
        public object[] EventState { get; set; }
    }
}
