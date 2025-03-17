using sint32 = int;

namespace Lib.Core.Attributes.Serialization;

public class BitSizeAttribute(int bitSize) : Attribute
{
    public sint32 BitSize { get; } = bitSize;
}