using Lib.Core.Attributes.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using RoomID = short;
using sint32 = int;
using uint16 = ushort;
using uint32 = uint;

namespace Lib.Core.Entities.Shared.Users;

[ByteSize(128)]
public class RegistrationRec : IStruct
{
    public sint32 AuxFlags;
    public uint32 Counter;
    public uint32 Crc;
    public uint32 DemoElapsed;
    public uint32 DemoLimit;
    public RoomID DesiredRoom;
    public uint32 PuidCRC;
    public uint32 PuidCtr;

    [ByteSize(6)] public string? Reserved;

    public uint32 TotalElapsed;
    public Upload2DEngineCaps Ul2DEngineCaps;
    public Upload2DGraphicsCaps Ul2DGraphicsCaps;
    public Upload3DEngineCaps Ul3DEngineCaps;
    public DownloadCapabilities UlDownloadCaps;

    public uint16 UlRequestedProtocolVersionMajorVersion;
    public uint16 UlRequestedProtocolVersionMinorVersion;
    public UploadCapabilities UlUploadCaps;

    [Str31] public string? UserName;

    [EncryptedString(1, 31, 32)] public string? WizPassword;
}