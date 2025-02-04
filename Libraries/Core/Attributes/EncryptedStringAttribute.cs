using ThePalace.Core.Enums;
using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class EncryptedStringAttribute(sint32 lengthByteSize = 1, sint32 maxStringLength = 255, sint32 paddingModulo = 0, EncryptedStringOptions opts = EncryptedStringOptions.None) : PStringAttribute
    {
        private readonly EncryptedStringOptions _opts = opts;

        public EncryptedStringOptions Options => _opts;
    }
}