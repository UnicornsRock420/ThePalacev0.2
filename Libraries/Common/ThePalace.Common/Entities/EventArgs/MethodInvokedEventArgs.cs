using System.Reflection;

namespace ThePalace.Common.Entities.EventArgs;

public class MethodInvokedEventArgs : System.EventArgs
{
    public MethodInfo? Member { get; internal set; }
    public string? Name => Member?.Name;
    public object[]? Args { get; internal set; }
}