using sint32 = int;

namespace ThePalace.Core.Attributes.Serialization;

public class DynamicSizeAttribute(sint32 maxByteSize = 0x7FFFF, sint32 minByteSize = 0, sint32 modulo = 0) : Attribute
{
    public sint32 MaxByteSize { get; } = maxByteSize <= minByteSize ? minByteSize : maxByteSize;

    public sint32 MinByteSize { get; } = minByteSize <= 0 ? 0 : minByteSize;

    public sint32 Modulo { get; } = modulo;
}