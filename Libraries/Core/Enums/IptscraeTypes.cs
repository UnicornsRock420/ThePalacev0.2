using ThePalace.Core.Attributes.Core;

namespace ThePalace.Core.Enums;

public enum IptEventTypes : short
{
    // IptVersion 1 Events
    [Version(1)] Alarm,
    [Version(1)] Chat,
    [Version(1)] Enter,
    [ScreenRefresh] [Version(1)] InChat,
    [Version(1)] Leave,
    [ScreenRefresh] [Version(1)] Lock,
    [Version(1)] Macro,
    [Version(1)] OutChat,
    [Version(1)] Select,

    [ScreenRefresh] [UIRefresh] [Version(1)]
    SignOn,
    [ScreenRefresh] [Version(1)] UnLock,

    // IptVersion 2 Events
    [ScreenRefresh] [Version(2)] ColorChange,
    [ScreenRefresh] [Version(2)] FaceChange,
    [Version(2)] FrameChange,
    [Version(2)] HttpError,
    [Version(2)] HttpReceived,
    [Version(2)] HttpSendprogress,
    [Version(2)] HttpReceiveprogress,
    [Version(2)] KeyDown,
    [Version(2)] KeyUp,
    [Version(2)] Idle,
    [ScreenRefresh] [Version(2)] LoosePropAdded,
    [ScreenRefresh] [Version(2)] LoosePropMoved,
    [ScreenRefresh] [Version(2)] LoosePropDeleted,
    [Version(2)] MouseDrag,
    [Version(2)] MouseDown,
    [Version(2)] MouseMove,
    [Version(2)] MouseUp,
    [ScreenRefresh] [Version(2)] NameChange,
    [Version(2)] RollOver,
    [Version(2)] RollOut,

    [ScreenRefresh] [UIRefresh] [Version(2)]
    RoomLoad,
    [Version(2)] RoomReady,
    [Version(2)] ServerMsg,
    [ScreenRefresh] [Version(2)] StateChange,

    [ScreenRefresh] [UIRefresh] [Version(2)]
    UserEnter,

    [ScreenRefresh] [UIRefresh] [Version(2)]
    UserLeave,
    [ScreenRefresh] [Version(2)] UserMove,
    [Version(2)] WebStatus,
    [Version(2)] WebTitle,
    [Version(2)] WebDocBegin,
    [Version(2)] WebDocDone,

    // IptVersion 3 Events
    [ScreenRefresh] [Version(3)] MsgAssetSend,
    [ScreenRefresh] [Version(3)] MsgDraw,
    [ScreenRefresh] [Version(3)] MsgSpotNew,
    [ScreenRefresh] [Version(3)] MsgSpotDel,
    [ScreenRefresh] [Version(3)] MsgSpotMove,
    [ScreenRefresh] [Version(3)] MsgPictNew,
    [ScreenRefresh] [Version(3)] MsgPictMove,
    [ScreenRefresh] [Version(3)] MsgPictDel,
    [ScreenRefresh] [Version(3)] MsgUserDesc,
    [ScreenRefresh] [Version(3)] MsgUserProp,

    [ScreenRefresh] [UIRefresh] [Version(3)]
    MsgUserLog,

    [ScreenRefresh] [UIRefresh] [Version(3)]
    MsgUserList,

    [ScreenRefresh] [UIRefresh] [Version(3)]
    MsgUserStatus,
    [ScreenRefresh] [Version(3)] MsgHttpServer,
    [Version(3)] MsgListOfAllUsers,
    [Version(3)] MsgListOfAllRooms,
    [UIRefresh] [Version(3)] MsgServerInfo,
    [Version(3)] MsgNavError,
    [Version(3)] UnhandledError,
    [Version(3)] ConnectionEstablished,
    [Version(3)] ConnectionError,
    [Version(3)] Disconnect,
    [Version(3)] SignOff,
    [Version(3)] Main
}

public enum IptVariableTypes
{
    Shadow, //Hidden
    Bool,
    Integer,
    Decimal,
    String,
    Array,
    Atomlist,
    Operator,
    Command,
    Variable,
    Object,
    Disposable
}

[Flags]
public enum IptOperatorFlags
{
    None = 0x00000000,
    Unary = 0x00000001,
    Boolean = 0x00000002,
    Assigning = 0x00000004,
    Push = 0x00000008,
    NOT = 0x00000010,
    OR = 0x00000020,
    AND = 0x00000040,
    XOR = 0x00000080,
    Math = 0x00000100,
    Add = 0x00000200,
    Subtract = 0x00000400,
    Multiply = 0x00000800,
    Divide = 0x00001000,
    Modulo = 0x00002000,
    Comparator = 0x00004000,
    EqualTo = 0x00008000,
    NotEqualTo = 0x00010000,
    GreaterThan = 0x00020000,
    LessThan = 0x00040000,
    Concate = 0x00080000,
    Coalesce = 0x00100000
}

[Flags]
public enum IptMetaVariableFlags
{
    None = 0,
    IsGlobal = 0x01,
    IsReadOnly = 0x02,
    IsSpecial = 0x04,
    All = IsGlobal | IsReadOnly | IsSpecial,
}

[Flags]
public enum IptTrackingFlags
{
    None = 0,
    Break = 0x01,
    Return = 0x02,
}