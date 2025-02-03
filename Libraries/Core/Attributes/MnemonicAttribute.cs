using uint32 = System.UInt32;

namespace ThePalace.Core.Attributes
{
    public class MnemonicAttribute(
        string mnemonic) : Attribute
    {
        private readonly string _mnemonic = mnemonic;
        private readonly uint32 _hexValue = BitConverter.ToUInt32(
            mnemonic.GetBytes(4).ToArray());

        public string Mnemonic => _mnemonic;
        public uint32 HexValue => _hexValue;
    }
}