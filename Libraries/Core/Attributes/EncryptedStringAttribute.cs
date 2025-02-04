using ThePalace.Core.Enums;
using sint32 = System.Int32;

namespace ThePalace.Core.Attributes
{
    public class EncryptedStringAttribute(
        sint32 lengthByteSize = 1,
        sint32 maxStringLength = 255,
        sint32 paddingModulo = 0,
        EncryptedStringOptions deserializeOptions = EncryptedStringOptions.DecryptString,
        EncryptedStringOptions serializeOptions = EncryptedStringOptions.EncryptString) : PStringAttribute
    {
        private readonly EncryptedStringOptions _deserializeOptions = deserializeOptions;
        private readonly EncryptedStringOptions _serializeOptions = serializeOptions;

        public EncryptedStringOptions DeserializeOptions => _deserializeOptions;
        public EncryptedStringOptions SerializeOptions => _serializeOptions;
    }
}