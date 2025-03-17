using System.Reflection;

namespace ThePalace.Common.Entities.EventArgs;

public class ExceptionEventArgs : System.EventArgs
{
    public Type? ClassType { get; internal set; }
    public MemberInfo? Member { get; internal set; }
    public Exception? Exception { get; internal set; }
    public string? Message => Exception?.Message;
    public string? StackTrace => Exception?.StackTrace;
}