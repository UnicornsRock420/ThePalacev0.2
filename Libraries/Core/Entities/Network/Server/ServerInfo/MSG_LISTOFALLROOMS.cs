using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts.Palace;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [DynamicSize]
    [Mnemonic("rLst")]
    public partial class MSG_LISTOFALLROOMS : EventParams, IStructRefNum, IStructSerializer, IProtocolS2C
    {
        [IgnoreDataMember]
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

        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.Rooms = [];

            while ((reader.Length - reader.Position) >= 12)
            {
                var room = new ListRec();
                reader.PalaceDeserialize(room, typeof(ListRec), opts);
                this.Rooms.Add(room);
            }
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            if ((this.Rooms?.Count ?? 0) > 0)
                foreach (var room in this.Rooms)
                    writer.PalaceSerialize(room, typeof(ListRec), opts);
        }
    }
}