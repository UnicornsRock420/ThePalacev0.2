using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class EncryptedStringAttribute(sint32 lengthByteSize = 1, sint32 maxStringLength = 255, sint32 paddingModulo = 0) : PStringAttribute
    {
    }
}