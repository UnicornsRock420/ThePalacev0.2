using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using HotspotID = System.Int16;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    [ByteSize(48)]
    public partial class HotspotRec : IProtocol
    {
        public sint32 ScriptEventMask;
        public sint32 Flags;
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

        public List<HotspotStateRec>? States;
        public List<Point>? Vortexes;

        [CString]
        public string? Name;
        [CString]
        public string? Script;
    }
}