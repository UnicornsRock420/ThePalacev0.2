using System.Drawing;
using System.Runtime.Serialization;
using Lib.Core.Attributes.Strings;
using Lib.Core.Enums;
using Point = Lib.Core.Entities.Shared.Types.Point;
using sint16 = short;
using uint8 = byte;

namespace Lib.Core.Entities.Shared.Rooms;

public partial class DrawCmdDesc
{
    [IgnoreDataMember] public uint8 Blue;

    [IgnoreDataMember] public uint8 Green;

    [IgnoreDataMember] public sint16 PenSize;

    [IgnoreDataMember] public List<Point>? Points;

    [IgnoreDataMember] public Point? Pos;

    [IgnoreDataMember] public Rectangle Rect;

    [IgnoreDataMember] public uint8 Red;

    [IgnoreDataMember] [CString] public string Text;

    [IgnoreDataMember]
    public DrawCmdTypes Type
    {
        get => (DrawCmdTypes)(DrawCmd & 0x00FF);
        set => DrawCmd = (short)((DrawCmd & 0xFF00) | ((short)value & 0x00FF));
    }

    [IgnoreDataMember]
    public bool Layer
    {
        get => (DrawCmd & 0x8000) != 0;
        set => DrawCmd = (short)((DrawCmd & 0x00FF) | (value ? 0x8000 : 0x0000));
    }

    [IgnoreDataMember]
    public bool Filled
    {
        get => (DrawCmd & 0x0100) != 0;
        set => DrawCmd = (short)((DrawCmd & 0x00FF) | (value ? 0x0100 : 0x0000));
    }
}