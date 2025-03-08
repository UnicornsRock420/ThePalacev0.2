using sint32 = int;

namespace ThePalace.Core.Attributes.Serialization;

public class ByteSizeAttribute(int byteSize) : Attribute
{
    public sint32 ByteSize { get; } = byteSize;
}