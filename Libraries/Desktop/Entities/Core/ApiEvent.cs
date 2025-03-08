namespace ThePalace.Common.Desktop.Entities.Core;

public class ApiEvent : EventArgs
{
    public Keys Keys { get; set; }
    public object Sender { get; set; }
    public object[] HotKeyState { get; set; }
    public object[] EventState { get; set; }
}