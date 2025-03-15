namespace ThePalace.Common.Entities.EventArgs;

public class ExceptionEventArgs : System.EventArgs
{
    public Exception Exception { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
}