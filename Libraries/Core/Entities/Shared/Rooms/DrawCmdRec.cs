using ThePalace.Core.Attributes.Serialization;
using sint16 = short;
using uint16 = ushort;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(10)]
public class DrawCmdRec
{
    public uint16 CmdLength;
    public sint16 DataOfst;
    public sint16 DrawCmd;
    public sint16 NextOfst;
    public sint16 Reserved;
}