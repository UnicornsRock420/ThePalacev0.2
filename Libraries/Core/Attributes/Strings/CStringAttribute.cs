using sint32 = int;

namespace Lib.Core.Attributes.Strings;

public class CStringAttribute(sint32 maxStringLength = 0x7FFF) : Attribute
{
    public sint32 MaxStringLength { get; } = maxStringLength;
}