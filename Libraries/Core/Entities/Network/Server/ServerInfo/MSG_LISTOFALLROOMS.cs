using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces;
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

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.Rooms = [];

            while ((reader.Length - reader.Position) >= 12)
            {
                var room = new ListRec();
                reader.PalaceDeserialize(refNum, room, typeof(ListRec), opts);
                this.Rooms.Add(room);
            }

            if (this.Rooms.Count != refNum)
                throw new InvalidDataException(nameof(MSG_LISTOFALLROOMS) + "-S2C: Deserialization Error!");
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            refNum = this.Rooms?.Count ?? 0;

            if (refNum > 0)
                foreach (var room in this.Rooms)
                    writer.PalaceSerialize(out refNum, room, typeof(ListRec), opts);
        }
    }
}