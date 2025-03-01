using sint32 = System.Int32;

namespace ThePalace.Core.Attributes.Serialization;

public class BitSizeAttribute(int bitSize) : Attribute
{
    private readonly sint32 _bitSize = bitSize;

    public sint32 BitSize => _bitSize;
}