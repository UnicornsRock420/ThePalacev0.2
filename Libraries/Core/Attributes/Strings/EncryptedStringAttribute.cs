using Lib.Core.Enums;
using sint32 = int;

namespace Lib.Core.Attributes.Strings;

public class EncryptedStringAttribute : PStringAttribute
{
    private EncryptedStringAttribute()
    {
    }

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
        DeserializeOptions = deserializeOptions;
        SerializeOptions = serializeOptions;
    }

    public EncryptedStringOptions DeserializeOptions { get; }

    public EncryptedStringOptions SerializeOptions { get; }
}