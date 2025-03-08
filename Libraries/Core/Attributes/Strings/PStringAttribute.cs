using sint32 = int;

namespace ThePalace.Core.Attributes.Strings;

public class PStringAttribute(sint32 lengthByteSize = 1, sint32 maxStringLength = 255, sint32 paddingModulo = 0)
    : Attribute
{
    public sint32 LengthByteSize { get; } = lengthByteSize;

    public sint32 MaxStringLength { get; } = maxStringLength;

    public sint32 PaddingModulo { get; } = paddingModulo;
}