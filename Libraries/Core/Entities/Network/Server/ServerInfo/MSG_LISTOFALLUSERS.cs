using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [DynamicSize]
    [Mnemonic("uLst")]
    public partial class MSG_LISTOFALLUSERS : IStructRefNum, IStructSerializer, IProtocolS2C
    {
        [RefNum]
        public sint32 RefNum
        {
            get => this.Users?.Count ?? 0;
            set
            {
                if (value > 0) return;

                this.Users = [];
            }
        }

        public List<ListRec>? Users;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.Users = [];

            while ((reader.Length - reader.Position) >= 12)
            {
                var user = new ListRec();
                reader.PalaceDeserialize(refNum, user, typeof(ListRec), opts);
                this.Users.Add(user);
            }

            if (this.Users.Count != refNum)
                throw new InvalidDataException(nameof(MSG_LISTOFALLUSERS) + "-S2C: Deserialization Error!");
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            refNum = this.Users?.Count ?? 0;

            if (refNum > 0)
                foreach (var user in this.Users)
                    writer.PalaceSerialize(out refNum, user, typeof(ListRec), opts);
        }
    }
}