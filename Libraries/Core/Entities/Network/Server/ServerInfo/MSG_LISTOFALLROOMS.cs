using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [DynamicSize]
    [Mnemonic("rLst")]
    public partial class MSG_LISTOFALLROOMS : IStructRefNum, IStructSerializer, IProtocolS2C
    {
        [RefNum]
        public sint32 RefNum
        {
            get => this.Rooms?.Count ?? 0;
            set
            {
                if (value > 0) return;

                this.Rooms = [];
            }
        }

        public List<ListRec>? Rooms;

        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.Rooms = [];

            while ((reader.Length - reader.Position) >= 12)
            {
                var room = new ListRec();
                reader.PalaceDeserialize(ref refNum, room, typeof(ListRec), opts);
                this.Rooms.Add(room);
            }

            if (this.Rooms.Count != refNum)
                throw new InvalidDataException(nameof(MSG_LISTOFALLROOMS) + "-S2C: Deserialization Error!");
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            if ((this.Rooms?.Count ?? 0) > 0)
                foreach (var room in this.Rooms)
                    writer.PalaceSerialize(ref refNum, room, typeof(ListRec), opts);
        }
    }
}