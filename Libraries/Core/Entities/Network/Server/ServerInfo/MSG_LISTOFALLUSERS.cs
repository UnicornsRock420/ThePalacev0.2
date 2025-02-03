using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [Mnemonic("uLst")]
    [DynamicSize]
    public partial class MSG_LISTOFALLUSERS : IProtocolRefNumOverride, IProtocolS2C, IProtocolSerializer
    {
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

            while ((reader.Length - reader.Position) >= 9)
            {
                var item = new ListRec();
                item.Deserialize(refNum, reader, opts);
                this.Users.Add(item);
            }

            if (this.Users.Count != refNum)
                throw new InvalidDataException(nameof(MSG_LISTOFALLUSERS) + "-S2C: Deserialization Error!");
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            if ((this.Users?.Count ?? 0) > 0)
                foreach (var user in this.Users)
                    user.Serialize(writer, opts);
        }
    }
}