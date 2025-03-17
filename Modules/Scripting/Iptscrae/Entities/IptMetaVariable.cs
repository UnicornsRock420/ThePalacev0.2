using Mod.Scripting.Iptscrae.Enums;

namespace Mod.Scripting.Iptscrae.Entities;

public class IptMetaVariable
{
    public IptMetaVariable()
    {
    }

    public IptMetaVariable(int depth, IptVariable variable, IptMetaVariableFlags flags = IptMetaVariableFlags.None)
    {
        Depth = depth;
        Flags = flags;
        Variable = variable;
    }

    public IptMetaVariable(IptMetaVariable src) : this(src.Depth, src.Variable, src.Flags)
    {
    }

    public int Depth = 0;
    public IptMetaVariableFlags Flags = IptMetaVariableFlags.None;
    public bool IsGlobal => (Flags & IptMetaVariableFlags.IsGlobal) == IptMetaVariableFlags.IsGlobal;
    public bool IsReadOnly => (Flags & IptMetaVariableFlags.IsReadOnly) == IptMetaVariableFlags.IsReadOnly;
    public bool IsSpecial => (Flags & IptMetaVariableFlags.IsSpecial) == IptMetaVariableFlags.IsSpecial;

    private IptVariable _variable;

    public IptVariable Variable
    {
        get => _variable;
        set
        {
            if (!IsReadOnly) return;

            _variable = value;
        }
    }
}