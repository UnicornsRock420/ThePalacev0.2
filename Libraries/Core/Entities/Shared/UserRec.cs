using ThePalace.Core.Attributes;
using ThePalace.Core.Factories;
using ThePalace.Core.Interfaces;
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
            this.PropSpec = new AssetSpec[9];
        }

        public void Dispose()
        {
            PropSpec = null;

            Extended
                ?.Values
                ?.Where(_ => _ is IDisposable)
                ?.Cast<IDisposable>()
                ?.ToList()
                ?.ForEach(_ => TCF
                    .Options(false)
                    .Try(() => _.Dispose())
                    .Execute());
            Extended?.Clear();
            Extended = null;

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

        [PString(1, 31)]
        public string? Name;
    }
}