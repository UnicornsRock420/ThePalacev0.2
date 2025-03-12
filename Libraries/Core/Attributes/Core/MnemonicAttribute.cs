using uint32 = uint;

namespace ThePalace.Core.Attributes.Core;

public class MnemonicAttribute(
    string mnemonic) : Attribute
{
    public string Mnemonic { get; } = mnemonic;

    public uint32 HexValue { get; } = BitConverter.ToUInt32(
        mnemonic.GetBytes(4).ToArray());
}