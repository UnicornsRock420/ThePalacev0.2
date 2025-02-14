using ThePalace.Core.Enums;
using sint32 = System.Int32;

namespace ThePalace.Core.Attributes.Strings
{
    public class EncryptedStringAttribute : PStringAttribute
    {
        private EncryptedStringAttribute() { }
        public EncryptedStringAttribute(
            sint32 lengthByteSize = 1,
            sint32 maxStringLength = 255,
            sint32 paddingModulo = 0,
            EncryptedStringOptions deserializeOptions = EncryptedStringOptions.DecryptString,
            EncryptedStringOptions serializeOptions = EncryptedStringOptions.EncryptString) : base(
                lengthByteSize,
                maxStringLength,
                paddingModulo)
        {
            _deserializeOptions = deserializeOptions;
            _serializeOptions = serializeOptions;
        }

        private readonly EncryptedStringOptions _deserializeOptions;
        private readonly EncryptedStringOptions _serializeOptions;

        public EncryptedStringOptions DeserializeOptions => _deserializeOptions;
        public EncryptedStringOptions SerializeOptions => _serializeOptions;
    }
}