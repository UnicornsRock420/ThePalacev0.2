using System.Reflection;

namespace Lib.Common.Entities.EventArgs;

public class MemberAccessedEventArgs : System.EventArgs
{
    public Type? ClassType { get; internal set; }
    public MemberInfo? Member { get; internal set; }
    public string? Name => Member?.Name;
}