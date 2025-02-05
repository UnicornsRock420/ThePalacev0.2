using ThePalace.Core.Attributes;
using sint32 = System.Int32;
using uint32 = System.UInt32;

namespace ThePalace.Core.Enums.Palace
{
    [Flags]
    [ByteSize(2)]
    public enum ServerPermissions : sint32
    {
        PM_AllowGuests = 0x0001,
        PM_AllowCyborgs = 0x0002,
        PM_AllowPainting = 0x0004,
        PM_AllowCustomProps = 0x0008,
        PM_AllowWizards = 0x0010,
        PM_WizardsMayKill = 0x0020,
        PM_WizardsMayAuthor = 0x0040,
        PM_PlayersMayKill = 0x0080,
        PM_CyborgsMayKill = 0x0100,
        PM_DeathPenalty = 0x0200,
        PM_PurgeInactiveProps = 0x0400,
        PM_KillFlooders = 0x0800,
        PM_NoSpoofing = 0x1000,
        PM_MemberCreateRooms = 0x2000,
    };

    [Flags]
    [ByteSize(4)]
    public enum ServerOptions : uint32
    {
        SO_SaveSessionKeys = 0x00000001,
        SO_PasswordSecurity = 0x00000002,
        SO_ChatLog = 0x00000004,
        SO_NoWhisper = 0x00000008,
        SO_AllowDemoMembers = 0x00000010,
        SO_Authenticate = 0x00000020,
        SO_PoundProtect = 0x00000040,
        SO_SortOptions = 0x00000080,
        SO_AuthTrackLogOff = 0x00000100,
        SO_JavaSecure = 0x00000200,
    };

    [ByteSize(4)]
    public enum ServerDownFlags : sint32
    {
        SD_Unknown = 0,
        SD_LoggedOff = 1,
        SD_CommError = 2,
        SD_Flood = 3,
        SD_KilledByPlayer = 4,
        SD_ServerDown = 5,
        SD_Unresponsive = 6,
        SD_KilledBySysop = 7,
        SD_ServerFull = 8,
        SD_InvalidSerialNumber = 9,
        SD_DuplicateUser = 10,
        SD_DeathPenaltyActive = 11,
        SD_Banished = 12,
        SD_BanishKill = 13,
        SD_NoGuests = 14,
        SD_DemoExpired = 15,
        SD_Verbose = 16,
    };
}