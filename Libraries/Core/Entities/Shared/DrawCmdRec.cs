using ThePalace.Core.Attributes;
using ThePalace.Core.Interfaces.Data;
using sint16 = System.Int16;
using uint16 = System.UInt16;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(10)]
    public partial class DrawCmdRec : RawData, IStruct
    {
        public DrawCmdRec()
        {
            _data = new();
        }
        public DrawCmdRec(uint8[]? data = null)
        {
            _data = new(data);
        }

        public override void Dispose()
        {
            _data?.Clear();
            _data = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }

        public sint16 NextOfst;
        public sint16 Reserved;
        public sint16 DrawCmd;
        public uint16 CmdLength;
        public sint16 DataOfst;
    }
}