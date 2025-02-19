﻿using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Enums.Palace
{
    [ByteSize(2)]
    public enum IptEventTypes : short
    {
        // IptVersion 1 Events
        Alarm,
        Chat,
        Enter,
        InChat,
        Leave,
        Lock,
        Macro,
        OutChat,
        Select,
        SignOn,
        UnLock,
        // IptVersion 2 Events
        ColorChange,
        FaceChange,
        FrameChange,
        HttpError,
        HttpReceived,
        HttpSendprogress,
        HttpReceiveprogress,
        KeyDown,
        KeyUp,
        Idle,
        LoosePropAdded,
        LoosePropMoved,
        LoosePropDeleted,
        MouseDrag,
        MouseDown,
        MouseMove,
        MouseUp,
        NameChange,
        RollOver,
        RollOut,
        RoomLoad,
        RoomReady,
        ServerMsg,
        StateChange,
        UserEnter,
        UserLeave,
        UserMove,
        WebStatus,
        WebTitle,
        WebDocBegin,
        WebDocDone,
        // IptVersion 3 Events
        MsgAssetSend,
        MsgDraw,
        MsgSpotNew,
        MsgSpotDel,
        MsgSpotMove,
        MsgPictNew,
        MsgPictMove,
        MsgPictDel,
        MsgUserDesc,
        MsgUserProp,
        MsgUserLog,
        MsgUserList,
        MsgUserStatus,
        MsgHttpServer,
        MsgListOfAllUsers,
        MsgListOfAllRooms,
        MsgServerInfo,
        MsgNavError,
        UnhandledError,
        ConnectionEstablished,
        ConnectionError,
        Disconnect,
        SignOff,
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
    [ByteSize(4)]
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
        Mod = 0x00002000,
        Comparator = 0x00004000,
        EqualTo = 0x00008000,
        NotEqualTo = 0x00010000,
        GreaterThan = 0x00020000,
        LessThan = 0x00040000,
        Concate = 0x00080000,
    }
}