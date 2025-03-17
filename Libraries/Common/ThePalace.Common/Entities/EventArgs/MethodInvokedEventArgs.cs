using System.Reflection;

namespace ThePalace.Common.Entities.EventArgs;

public class MethodInvokedEventArgs : System.EventArgs
{
    public Type? ClassType { get; internal set; }
    public MethodInfo? Method { get; internal set; }
    public string? Name => Method?.Name;
    public object[]? Args { get; internal set; }
}