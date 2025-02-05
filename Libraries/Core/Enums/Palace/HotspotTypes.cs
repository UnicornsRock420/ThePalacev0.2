using System.ComponentModel;
using ThePalace.Core.Attributes;

namespace ThePalace.Core.Enums.Palace
{
    [ByteSize(4)]
    public enum HotSpotEventMask : int
    {
        PE_Select = 0x00000001,
        PE_Lock = 0x00000002,
        PE_Unlock = 0x00000004,
        PE_Hide = 0x00000008,
        PE_Show = 0x00000010,
        PE_Startup = 0x00000020,
        PE_Alarm = 0x00000040,
        PE_Custom = 0x00000080,
        PE_InChat = 0x00000100,
        PE_PropChange = 0x00000200,
        PE_Enter = 0x00000400,
        PE_Leave = 0x00000800,
        PE_OutChat = 0x00001000,
        PE_SignOn = 0x00002000,
        PE_SignOff = 0x00004000,
        PE_Macro0 = 0x00008000,
        PE_Macro1 = 0x00010000,
        PE_Macro2 = 0x00020000,
        PE_Macro3 = 0x00040000,
        PE_Macro4 = 0x00080000,
        PE_Macro5 = 0x00100000,
        PE_Macro6 = 0x00200000,
        PE_Macro7 = 0x00400000,
        PE_Macro8 = 0x00800000,
        PE_Macro9 = 0x01000000,
    };

    [Flags]
    [ByteSize(4)]
    public enum HotspotFlags : int
    {
        None = 0,
        [Description("DRAGGABLE")]
        HS_Draggable = 0x0001,
        [Description("DONTMOVEHERE")]
        HS_DontMoveHere = 0x0002,
        [Description("INVISIBLE")]
        HS_Invisible = 0x0004,
        [Description("SHOWNAME")]
        HS_ShowName = 0x0008,
        [Description("SHOWFRAME")]
        HS_ShowFrame = 0x0010,
        [Description("SHADOW")]
        HS_Shadow = 0x0020,
        [Description("FILL")]
        HS_Fill = 0x0040,
        [Description("FORBIDDEN")]
        HS_Forbidden = 0x0080,
        [Description("MANDATORY")]
        HS_Mandatory = 0x0100,
        [Description("LANDINGPAD")]
        HS_LandingPad = 0x0200,
    };

    [ByteSize(2)]
    public enum HotspotTypes : short
    {
        [Description("SPOT")]
        HS_Normal = 0,
        [Description("DOOR")]
        HS_Door = 1,
        [Description("DOOR")]
        HS_ShutableDoor = 2,
        [Description("DOOR")]
        HS_LockableDoor = 3,
        [Description("BOLT")]
        HS_Bolt = 4,
        [Description("NAVAREA")]
        HS_NavArea = 5
    };

    [ByteSize(2)]
    public enum HotspotStates : short
    {
        HS_Unlock = 0,
        HS_Lock = 1,
    };
}