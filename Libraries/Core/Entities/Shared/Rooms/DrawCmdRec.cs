using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;
using uint16 = System.UInt16;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(10)]
    public partial class DrawCmdRec : RawStream, IStruct
    {
        public sint16 NextOfst;
        public sint16 Reserved;
        public sint16 DrawCmd;
        public uint16 CmdLength;
        public sint16 DataOfst;
    }
}