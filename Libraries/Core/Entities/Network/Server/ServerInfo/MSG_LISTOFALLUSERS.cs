using System.Runtime.Serialization;
using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;
using ThePalace.Core.Entities.EventsBus;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Exts;
using ThePalace.Core.Interfaces.Data;
using ThePalace.Core.Interfaces.Network;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [DynamicSize]
    [Mnemonic("uLst")]
    public partial class MSG_LISTOFALLUSERS : EventParams, IStructRefNum, IStructSerializer, IProtocolS2C
    {
        [IgnoreDataMember]
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

        public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.Users = [];

            while ((reader.Length - reader.Position) >= 12)
            {
                var user = new ListRec();
                reader.PalaceDeserialize(user, typeof(ListRec), opts);
                this.Users.Add(user);
            }
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            if ((this.Users?.Count ?? 0) > 0)
                foreach (var user in this.Users)
                    writer.PalaceSerialize(user, typeof(ListRec), opts);
        }
    }
}