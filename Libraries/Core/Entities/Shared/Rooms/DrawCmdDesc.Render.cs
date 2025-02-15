using System.Drawing;
using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Enums.Palace;
using sint16 = System.Int16;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared.Rooms
{
    public partial class DrawCmdDesc
    {
        [IgnoreDataMember]
        public DrawCmdTypes Type
        {
            get => (DrawCmdTypes)(DrawCmdInfo.DrawCmd & 0x00FF);
            set => DrawCmdInfo.DrawCmd = (short)(DrawCmdInfo.DrawCmd & 0xFF00 | (short)value & 0x00FF);
        }
        [IgnoreDataMember]
        public bool Layer
        {
            get => (DrawCmdInfo.DrawCmd & 0x8000) != 0;
            set => DrawCmdInfo.DrawCmd = (short)(DrawCmdInfo.DrawCmd & 0x00FF | (value ? 0x8000 : 0x0000));
        }
        [IgnoreDataMember]
        public bool Filled
        {
            get => (DrawCmdInfo.DrawCmd & 0x0100) != 0;
            set => DrawCmdInfo.DrawCmd = (short)(DrawCmdInfo.DrawCmd & 0x00FF | (value ? 0x0100 : 0x0000));
        }

        [IgnoreDataMember]
        public sint16 PenSize;
        [IgnoreDataMember]
        public uint8 Red;
        [IgnoreDataMember]
        public uint8 Green;
        [IgnoreDataMember]
        public uint8 Blue;
        [IgnoreDataMember]
        public Types.Point? Pos;

        [IgnoreDataMember]
        public List<Types.Point>? Points;
        [IgnoreDataMember]
        public Rectangle Rect;

        [IgnoreDataMember]
        [CString]
        public string Text;
    }
}