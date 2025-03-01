using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using RoomID = System.Int16;
using sint32 = System.Int32;
using uint16 = System.UInt16;
using uint32 = System.UInt32;

namespace ThePalace.Core.Entities.Shared.Users;

[ByteSize(128)]
public partial class RegistrationRec : IStruct
{
    public uint32 Crc;
    public uint32 Counter;

    [Str31]
    public string? UserName;

    [EncryptedString(1, 31, 32)]
    public string? WizPassword;

    public sint32 AuxFlags;
    public uint32 PuidCtr;
    public uint32 PuidCRC;
    public uint32 DemoElapsed;
    public uint32 TotalElapsed;
    public uint32 DemoLimit;
    public RoomID DesiredRoom;

    [ByteSize(6)]
    public string? Reserved;

    public uint16 UlRequestedProtocolVersionMajorVersion;
    public uint16 UlRequestedProtocolVersionMinorVersion;
    public UploadCapabilities UlUploadCaps;
    public DownloadCapabilities UlDownloadCaps;
    public Upload2DEngineCaps Ul2DEngineCaps;
    public Upload2DGraphicsCaps Ul2DGraphicsCaps;
    public Upload3DEngineCaps Ul3DEngineCaps;
}