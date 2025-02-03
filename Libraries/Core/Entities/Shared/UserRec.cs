using System.Collections.Concurrent;
using System.Runtime.Serialization;
using ThePalace.Core.Attributes;
using ThePalace.Core.Factories;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using RoomID = System.Int16;
using sint16 = System.Int16;
using UserID = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    public partial class UserRec : IDisposable, IProtocol
    {
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
        [DynamicSize(8 * 9)]
        public AssetSpec[] PropSpec;
        public RoomID RoomID;
        public sint16 FaceNbr;
        public sint16 ColorNbr;
        public sint16 AwayFlag;
        public sint16 OpenToMsgs;
        public sint16 NbrProps;
        public Str31 Name;

        [IgnoreDataMember]
        public ConcurrentDictionary<string, object> Extended;
    }
}