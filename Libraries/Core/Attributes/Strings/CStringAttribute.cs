using sint32 = int;

namespace ThePalace.Core.Attributes.Strings;

public class CStringAttribute(sint32 maxStringLength = 0x7FFF) : Attribute
{
    public sint32 MaxStringLength { get; } = maxStringLength;
}