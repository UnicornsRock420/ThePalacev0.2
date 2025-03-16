using System.Reflection;

namespace ThePalace.Common.Entities.EventArgs;

public class FieldAccessedEventArgs : System.EventArgs
{
    public MemberInfo? Member { get; internal set; }
    public string? Name => Member?.Name;
}