using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using HotspotID = System.Int16;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(48)]
    public partial class HotspotRec : IStruct
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
}