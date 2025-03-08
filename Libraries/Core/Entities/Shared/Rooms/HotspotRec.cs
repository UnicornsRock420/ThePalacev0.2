using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using HotspotID = short;
using sint16 = short;
using sint32 = int;

namespace ThePalace.Core.Entities.Shared.Rooms;

[ByteSize(48)]
public class HotspotRec : IStruct
{
    public sint16 AlignReserved;
    public sint16 Dest;
    public HotspotFlags Flags;
    public sint16 GroupID;
    public HotspotID HotspotID;
    public Point Loc;
    public sint16 NameOfst;
    public sint16 NbrPts;
    public sint16 NbrScripts;
    public sint16 NbrStates;
    public sint16 PtsOfst;
    public sint32 RefCon;
    public HotSpotEventMask ScriptEventMask;
    public sint16 ScriptRecOfst;
    public sint16 ScriptTextOfst;
    public sint32 SecureInfo;
    public sint16 State;
    public sint16 StateRecOfst;
    public HotspotTypes Type;
}