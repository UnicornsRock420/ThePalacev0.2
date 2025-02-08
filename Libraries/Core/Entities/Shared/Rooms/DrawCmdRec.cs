using ThePalace.Core.Attributes.Serialization;
using sint16 = System.Int16;
using uint16 = System.UInt16;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(10)]
    public partial class DrawCmdRec
    {
        public sint16 NextOfst;
        public sint16 Reserved;
        public sint16 DrawCmd;
        public uint16 CmdLength;
        public sint16 DataOfst;
    }
}