using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Enums.Palace
{
    public enum IptEventTypes : short
    {
        // IptVersion 1 Events
        [Version(1)]
        Alarm,
        [Version(1)]
        Chat,
        [Version(1)]
        Enter,
        [Version(1)]
        InChat,
        [Version(1)]
        Leave,
        [Version(1)]
        Lock,
        [Version(1)]
        Macro,
        [Version(1)]
        OutChat,
        [Version(1)]
        Select,
        [Version(1)]
        SignOn,
        [Version(1)]
        UnLock,
        // IptVersion 2 Events
        [Version(2)]
        ColorChange,
        [Version(2)]
        FaceChange,
        FrameChange,
        [Version(2)]
        HttpError,
        HttpReceived,
        [Version(2)]
        HttpSendprogress,
        HttpReceiveprogress,
        [Version(2)]
        KeyDown,
        [Version(2)]
        KeyUp,
        [Version(2)]
        Idle,
        [Version(2)]
        LoosePropAdded,
        [Version(2)]
        LoosePropMoved,
        [Version(2)]
        LoosePropDeleted,
        [Version(2)]
        MouseDrag,
        [Version(2)]
        MouseDown,
        [Version(2)]
        MouseMove,
        [Version(2)]
        MouseUp,
        [Version(2)]
        NameChange,
        [Version(2)]
        RollOver,
        [Version(2)]
        RollOut,
        [Version(2)]
        RoomLoad,
        [Version(2)]
        RoomReady,
        [Version(2)]
        ServerMsg,
        [Version(2)]
        StateChange,
        [Version(2)]
        UserEnter,
        [Version(2)]
        UserLeave,
        [Version(2)]
        UserMove,
        [Version(2)]
        WebStatus,
        [Version(2)]
        WebTitle,
        [Version(2)]
        WebDocBegin,
        [Version(2)]
        WebDocDone,
        // IptVersion 3 Events
        [Version(3)]
        MsgAssetSend,
        [Version(3)]
        MsgDraw,
        [Version(3)]
        MsgSpotNew,
        [Version(3)]
        MsgSpotDel,
        [Version(3)]
        MsgSpotMove,
        [Version(3)]
        MsgPictNew,
        [Version(3)]
        MsgPictMove,
        [Version(3)]
        MsgPictDel,
        [Version(3)]
        MsgUserDesc,
        [Version(3)]
        MsgUserProp,
        [Version(3)]
        MsgUserLog,
        [Version(3)]
        MsgUserList,
        [Version(3)]
        MsgUserStatus,
        [Version(3)]
        MsgHttpServer,
        [Version(3)]
        MsgListOfAllUsers,
        [Version(3)]
        MsgListOfAllRooms,
        [Version(3)]
        MsgServerInfo,
        [Version(3)]
        MsgNavError,
        [Version(3)]
        UnhandledError,
        [Version(3)]
        ConnectionEstablished,
        [Version(3)]
        ConnectionError,
        [Version(3)]
        Disconnect,
        [Version(3)]
        SignOff,
        [Version(3)]
        Main,
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
        Disposable,
    }

    [Flags]
    public enum IptOperatorFlags : int
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
        Coalesce = 0x00100000,
    }
}