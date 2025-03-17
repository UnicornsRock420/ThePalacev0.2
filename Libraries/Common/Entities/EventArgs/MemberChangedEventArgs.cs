namespace Lib.Common.Entities.EventArgs;

public class MemberChangedEventArgs : MemberAccessedEventArgs
{
    public object? OldValue { get; internal set; }
    public object? NewValue { get; internal set; }
}