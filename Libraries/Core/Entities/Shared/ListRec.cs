using ThePalace.Core.Attributes;
using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;
using ThePalace.Core.Types;
using sint16 = System.Int16;
using sint32 = System.Int32;

namespace ThePalace.Core.Entities.Shared
{
    [DynamicSize(40, 9)]
    public partial class ListRec : IProtocolSerializer
    {
        public ListRec()
        {
            this.Name = string.Empty;
        }

        public sint32 PrimaryID;
        public sint16 Flags;
        public sint16 RefNum;
        public string Name;

        public void Deserialize(int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            this.PrimaryID = reader.ReadInt32();
            this.Flags = reader.ReadInt16();
            this.RefNum = reader.ReadInt16();

            var length = reader.ReadByte();
            if (length < 1) return;

            var buffer = new byte[length];
            reader.Read(buffer, 0, buffer.Length);

            this.Name = buffer.GetString();

            var remaining = (length % 4) > 0 ? Math.Abs(4 - (length % 4)) : 0;
            if (remaining > 0)
            {
                buffer = new byte[remaining];
                reader.Read(buffer, 0, buffer.Length);
            }
        }

        public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            writer.WriteInt32(this.PrimaryID);
            writer.WriteInt16(this.Flags);
            writer.WriteInt16(this.RefNum);

            writer.Write(new PString(this.Name).Value);

            var length = this.Name.Length;
            var remaining = (length % 4) > 0 ? Math.Abs(4 - (length % 4)) : 0;
            if (remaining > 0)
            {
                var padBytes = new byte[remaining];
                writer.Write(padBytes);
            }
        }
    }

    //[MessagePackObject(true, AllowPrivate = true)]
    //public partial class UserListRec : ListRec
    //{
    //    [IgnoreDataMember]
    //    public sint32 UserID
    //    {
    //        get => PrimaryID;
    //        set => PrimaryID = value;
    //    }
    //    [IgnoreDataMember]
    //    public sint16 RoomID
    //    {
    //        get => RefNum;
    //        set => RefNum = value;
    //    }
    //}

    //[MessagePackObject(true, AllowPrivate = true)]
    //public partial class RoomListRec : ListRec
    //{
    //    [IgnoreDataMember]
    //    public sint32 RoomID
    //    {
    //        get => PrimaryID;
    //        set => PrimaryID = value;
    //    }
    //    [IgnoreDataMember]
    //    public sint16 NbrUsers
    //    {
    //        get => RefNum;
    //        set => RefNum = value;
    //    }
    //}
}