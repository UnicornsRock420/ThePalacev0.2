namespace ThePalace.Core.Enums;

[Flags]
public enum EncryptedStringOptions : short
{
    None = 0,
    EncryptString = 0x01,
    DecryptString = 0x02,
    FromHex = 0x04,
    ToHex = 0x08,
}