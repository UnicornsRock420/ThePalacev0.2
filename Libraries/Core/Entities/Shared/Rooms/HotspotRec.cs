using Lib.Core.Attributes.Serialization;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Enums;
using Lib.Core.Interfaces.Data;
using HotspotID = short;
using sint16 = short;
using sint32 = int;

namespace Lib.Core.Entities.Shared.Rooms;

[ByteSize(48)]
public class HotspotRec : IStruct
{
    public HotSpotEventMask ScriptEventMask;
    public HotspotFlags Flags;
    public sint32 SecureInfo;
    public sint32 RefCon;
    public Point Loc;
    public HotspotID HotspotID;
    public sint16 Dest;
    public sint16 NbrPts;
    public sint16 PtsOfst;
    public HotspotTypes Type;
    public sint16 GroupID;
    public sint16 NbrScripts;
    public sint16 ScriptRecOfst;
    public sint16 State;
    public sint16 NbrStates;
    public sint16 StateRecOfst;
    public sint16 NameOfst;
    public sint16 ScriptTextOfst;
    public sint16 AlignReserved;
}