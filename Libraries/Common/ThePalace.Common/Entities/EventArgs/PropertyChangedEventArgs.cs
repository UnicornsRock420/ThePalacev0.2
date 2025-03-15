using System.Reflection;

namespace ThePalace.Common.Entities.EventArgs;

public class PropertyChangedEventArgs : System.EventArgs
{
    public PropertyInfo Property { get; set; }
    public string? Name { get; set; }
    public object? Value { get; set; }
}