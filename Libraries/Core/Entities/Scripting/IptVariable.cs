using ThePalace.Core.Enums;

namespace ThePalace.Core.Entities.Scripting;

public class IptVariable
{
    public IptVariable()
    {
    }

    public IptVariable(IptVariableTypes type)
    {
        Type = type;
    }

    public IptVariable(
        IptVariableTypes type,
        object value)
    {
        Type = type;
        Value = value;
    }

    public IptVariableTypes Type { get; internal set; }
    public object Value { get; internal set; }

    public T GetValue<T>()
    {
        return (T)Value;
    }
}