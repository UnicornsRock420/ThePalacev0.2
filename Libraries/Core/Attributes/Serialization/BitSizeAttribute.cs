using sint32 = int;

namespace ThePalace.Core.Attributes.Serialization;

public class BitSizeAttribute(int bitSize) : Attribute
{
    public sint32 BitSize { get; } = bitSize;
}