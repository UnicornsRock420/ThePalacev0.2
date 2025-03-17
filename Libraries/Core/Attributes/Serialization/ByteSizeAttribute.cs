using sint32 = int;

namespace Lib.Core.Attributes.Serialization;

public class ByteSizeAttribute(int byteSize) : Attribute
{
    public sint32 ByteSize { get; } = byteSize;
}