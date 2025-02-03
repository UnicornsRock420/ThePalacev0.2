﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Network.Server.ServerInfo
{
    [Mnemonic("rLst")]
    [DynamicSize]
    public partial class MSG_LISTOFALLROOMS : IProtocolRefNumOverride, IProtocolS2C, IProtocolSerializer
    {
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

            while ((reader.Length - reader.Position) >= 9)
            {
                var item = new ListRec();
                item.Deserialize(refNum, reader, opts);
                this.Rooms.Add(item);
            }

            if (this.Rooms.Count != refNum)
                throw new InvalidDataException(nameof(MSG_LISTOFALLROOMS) + "-S2C: Deserialization Error!");
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            if ((this.Rooms?.Count ?? 0) > 0)
                foreach (var room in this.Rooms)
                    room.Serialize(writer, opts);
        }
    }
}