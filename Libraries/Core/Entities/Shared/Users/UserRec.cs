using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Attributes.Strings;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Types;
using RoomID = System.Int16;
using sint16 = System.Int16;
using UserID = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    public partial class UserRec : IDisposable, IStruct
    {
        public UserRec()
        {
            this.RoomPos = new();
            this.PropSpec = new AssetSpec[9];
        }

        public void Dispose()
        {
            PropSpec = null;

            GC.SuppressFinalize(this);
        }

        public UserID UserID;
        public Point RoomPos;

        [ByteSize(8 * 9)] // AssetSpec(8) * Props(9)
        public AssetSpec[] PropSpec;

        public RoomID RoomID;
        public sint16 FaceNbr;
        public sint16 ColorNbr;
        public sint16 AwayFlag;
        public sint16 OpenToMsgs;
        public sint16 NbrProps;

        [Str31]
        public string? Name;
    }
}