using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Enums.Palace
{
    [Flags]
    [ByteSize(4)]
    public enum AuthTypes : uint
    {
        Password = 0x01,
        IPAddress = 0x02,
        RegCode = 0x04,
        PUID = 0x08,
    };
}