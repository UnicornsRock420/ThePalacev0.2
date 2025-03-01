using System.ComponentModel;
using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Enums.Palace;

[Flags]
[ByteSize(4)]
public enum RoomFlags : int
{
    None = 0,
    AuthorLocked = 0x0001,
    [Description("PRIVATE")]
    Private = 0x0002,
    [Description("NOPAINTING")]
    NoPainting = 0x0004,
    Closed = 0x0008,
    [Description("NOCYBORGS")]
    CyborgFreeZone = 0x0010,
    [Description("HIDDEN")]
    Hidden = 0x0020,
    [Description("NOGUESTS")]
    NoGuests = 0x0040,
    [Description("OPERATORSONLY")]
    WizardsOnly = 0x0080,
    [Description("DROPZONE")]
    DropZone = 0x0100,
    [Description("NOLOOSEPROPS")]
    RF_NoLooseProps = 0x0200,
};

[ByteSize(4)]
public enum NavErrors : int
{
    SE_InternalError = 0,
    SE_RoomUnknown = 1,
    SE_RoomFull = 2,
    SE_RoomClosed = 3,
    SE_CantAuthor = 4,
    SE_PalaceFull = 5,
};