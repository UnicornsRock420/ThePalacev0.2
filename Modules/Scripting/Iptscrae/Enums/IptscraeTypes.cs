namespace Mod.Scripting.Iptscrae.Enums;

[Flags]
public enum IptVariableTypes : ushort
{
    None = 0,
    Hidden = 0x8000,
    Shadow = 0x8000,
    Bool = 0x0001,
    Integer = 0x0002,
    Decimal = 0x0004,
    String = 0x0008,
    Array = 0x0010,
    Atomlist = 0x0020,
    Operator = 0x0040,
    Command = 0x0080,
    Variable = 0x0100,
    Object = 0x0200,
    Disposable = 0x0400
}

[Flags]
public enum IptOperatorFlags : uint
{
    None = 0x00000000,
    Unary = 0x00000001,
    Boolean = 0x00000002,
    Assigning = 0x00000004,
    Push = 0x00000008,
    NOT = 0x00000010,
    OR = 0x00000020,
    AND = 0x00000040,
    XOR = 0x00000080,
    Math = 0x00000100,
    Add = 0x00000200,
    Subtract = 0x00000400,
    Multiply = 0x00000800,
    Divide = 0x00001000,
    Modulo = 0x00002000,
    Comparator = 0x00004000,
    EqualTo = 0x00008000,
    NotEqualTo = 0x00010000,
    GreaterThan = 0x00020000,
    LessThan = 0x00040000,
    Concate = 0x00080000,
    Coalesce = 0x00100000
}

[Flags]
public enum IptMetaVariableFlags : ushort
{
    None = 0,
    IsGlobal = 0x01,
    IsReadOnly = 0x02,
    IsSpecial = 0x04,
    All = IsGlobal | IsReadOnly | IsSpecial,
}

[Flags]
public enum IptTrackingFlags : ushort
{
    None = 0,
    Break = 0x01,
    Return = 0x02,
}