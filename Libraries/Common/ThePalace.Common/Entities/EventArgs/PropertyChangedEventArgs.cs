using System.Reflection;

namespace ThePalace.Common.Entities.EventArgs;

public class PropertyChangedEventArgs : System.EventArgs
{
    public MemberInfo? Member { get; internal set; }
    public string? Name => Member?.Name;
    public object? OldValue { get; internal set; }
    public object? NewValue { get; internal set; }
}