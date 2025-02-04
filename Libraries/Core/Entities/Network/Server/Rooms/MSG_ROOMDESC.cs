﻿using ThePalace.Core.Attributes;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Network.Server.Rooms
{
    [Mnemonic("room")]
    [DynamicSize]
    public partial class MSG_ROOMDESC : IProtocolS2C, IStructSerializer
    {
        public RoomRec? RoomInfo;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(out int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }
    }
}