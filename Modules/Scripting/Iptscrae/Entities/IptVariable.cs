using Mod.Scripting.Iptscrae.Enums;

namespace Mod.Scripting.Iptscrae.Entities;

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

    public IptVariableTypes Type { get; protected set; }
    public object Value { get; set; }

    public T GetValue<T>()
    {
        return (T)Value;
    }
}