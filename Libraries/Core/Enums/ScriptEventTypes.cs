using Lib.Common.Attributes.Core;
using Lib.Common.Attributes.UI;

namespace Lib.Core.Enums;

public enum ScriptEventTypes : short
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