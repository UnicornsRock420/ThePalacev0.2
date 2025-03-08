using ThePalace.Core.Attributes.Serialization;
using uint16 = ushort;
using uint32 = uint;

namespace ThePalace.Core.Enums;

[Flags]
[ByteSize(2)]
public enum UserFlags : uint16
{
    U_Moderator = 0x0001,
    U_Administrator = 0x0002,
    U_Kill = 0x0004,
    U_Guest = 0x0008,
    U_Banished = 0x0010,
    U_Penalized = 0x0020,
    U_CommError = 0x0040,
    U_Gag = 0x0080,
    U_Pin = 0x0100,
    U_Hide = 0x0200,
    U_RejectEsp = 0x0400,
    U_RejectWhisper = 0x0800,
    U_PropGag = 0x1000,
    U_NameGag = 0x2000
}

[Flags]
[ByteSize(4)]
public enum UserAuxFlags : uint32
{
    LI_AUXFLAGS_UnknownMach = 0,
    LI_AUXFLAGS_Mac68k = 1,
    LI_AUXFLAGS_MacPPC = 2,
    LI_AUXFLAGS_Win16 = 3,
    LI_AUXFLAGS_Win32 = 4,
    LI_AUXFLAGS_Java = 5,
    LI_AUXFLAGS_OSMask = 0x0000000F,
    LI_AUXFLAGS_Authenticate = 0x80000000
}

[Flags]
[ByteSize(4)]
public enum UploadCapabilities : uint32
{
    LI_ULCAPS_ASSETS_PALACE = 0x00000001,
    LI_ULCAPS_ASSETS_FTP = 0x00000002,
    LI_ULCAPS_ASSETS_HTTP = 0x00000004,
    LI_ULCAPS_ASSETS_OTHER = 0x00000008,
    LI_ULCAPS_FILES_PALACE = 0x00000010,
    LI_ULCAPS_FILES_FTP = 0x00000020,
    LI_ULCAPS_FILES_HTTP = 0x00000040,
    LI_ULCAPS_FILES_OTHER = 0x00000080,
    LI_ULCAPS_EXTEND_PKT = 0x00000100
}

[Flags]
[ByteSize(4)]
public enum DownloadCapabilities : uint32
{
    LI_DLCAPS_ASSETS_PALACE = 0x00000001,
    LI_DLCAPS_ASSETS_FTP = 0x00000002,
    LI_DLCAPS_ASSETS_HTTP = 0x00000004,
    LI_DLCAPS_ASSETS_OTHER = 0x00000008,
    LI_DLCAPS_FILES_PALACE = 0x00000010,
    LI_DLCAPS_FILES_FTP = 0x00000020,
    LI_DLCAPS_FILES_HTTP = 0x00000040,
    LI_DLCAPS_FILES_OTHER = 0x00000080,
    LI_DLCAPS_FILES_HTTPSrvr = 0x00000100,
    LI_DLCAPS_EXTEND_PKT = 0x00000200
}

[Flags]
[ByteSize(4)]
public enum Upload2DEngineCaps : uint32
{
    LI_2DENGINECAP_PALACE = 0x00000001,
    LI_2DENGINECAP_DOUBLEBYTE = 0x00000002
}

[Flags]
[ByteSize(4)]
public enum Upload2DGraphicsCaps : uint32
{
    LI_2DGRAPHCAP_GIF87 = 0x00000001,
    LI_2DGRAPHCAP_GIF89a = 0x00000002,
    LI_2DGRAPHCAP_JPG = 0x00000004,
    LI_2DGRAPHCAP_TIFF = 0x00000008,
    LI_2DGRAPHCAP_TARGA = 0x00000010,
    LI_2DGRAPHCAP_BMP = 0x00000020,
    LI_2DGRAPHCAP_PCT = 0x00000040
}

[Flags]
[ByteSize(4)]
public enum Upload3DEngineCaps : uint32
{
    LI_3DENGINECAP_VRML1 = 0x00000001,
    LI_3DENGINECAP_VRML2 = 0x00000002
}