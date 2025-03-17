namespace ThePalace.Common.Entities.EventArgs;

public class FieldChangedEventArgs : FieldAccessedEventArgs
{
    public object? OldValue { get; internal set; }
    public object? NewValue { get; internal set; }
}