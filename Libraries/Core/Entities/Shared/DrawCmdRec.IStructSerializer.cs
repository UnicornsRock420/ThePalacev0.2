using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Shared
{
    public partial class DrawCmdRec : IStructSerializer
    {
        public void Deserialize(ref int refNum, Stream reader, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Serialize(ref int refNum, Stream writer, SerializerOptions opts = SerializerOptions.None)
        {
            throw new NotImplementedException();
        }

        public void Deserialize()
        {
            //nextOfst = packet.ReadSInt16();
            //packet.DropBytes(2); //reserved
            //DrawCmd = packet.ReadSInt16();
            //cmdLength = packet.ReadUInt16();
            //dataOfst = packet.ReadSInt16();
            //data = packet.Data
            //    .Skip(dataOfst)
            //    .Take(cmdLength)
            //    .ToArray();
        }

        //public void DeserializeData()
        //{
        //    using (var packet = new Packet(data))
        //        switch (type)
        //        {
        //            case DrawCmdTypes.DC_Path:
        //                {
        //                    penSize = packet.ReadSInt16();
        //                    var nbrPoints = packet.ReadSInt16();
        //                    red = (byte)packet.ReadSInt16().SwapShort();
        //                    green = (byte)packet.ReadSInt16().SwapShort();
        //                    blue = (byte)packet.ReadSInt16().SwapShort();

        //                    pos = new();
        //                    pos.v = packet.ReadSInt16();
        //                    pos.h = packet.ReadSInt16();

        //                    Points = new();
        //                    while (Points.Count < nbrPoints &&
        //                        packet.Length >= Network.Protocols.Interfaces.Entities.Palace.Point.SizeOf)
        //                    {
        //                        var p = new Palace.Point();
        //                        p.v = packet.ReadSInt16();
        //                        p.h = packet.ReadSInt16();

        //                        Points.Add(p);
        //                    }
        //                }

        //                break;
        //            case DrawCmdTypes.DC_Ellipse:
        //                {
        //                    //penSize = packet.ReadSInt16();
        //                    //red = (byte)packet.ReadSInt16().SwapShort();
        //                    //green = (byte)packet.ReadSInt16().SwapShort();
        //                    //blue = (byte)packet.ReadSInt16().SwapShort();

        //                    //Rect = new();
        //                    //Rect.X = packet.ReadSInt16();
        //                    //Rect.Y = packet.ReadSInt16();
        //                    //Rect.Width = packet.ReadSInt16();
        //                    //Rect.Height = packet.ReadSInt16();

        //                    throw new NotImplementedException(nameof(DrawCmdTypes.DC_Ellipse));
        //                }

        //                break;
        //            case DrawCmdTypes.DC_Text:
        //                {
        //                    //penSize = packet.ReadSInt16();
        //                    //red = (byte)packet.ReadSInt16().SwapShort();
        //                    //green = (byte)packet.ReadSInt16().SwapShort();
        //                    //blue = (byte)packet.ReadSInt16().SwapShort();

        //                    //pos = new();
        //                    //pos.v = packet.ReadSInt16();
        //                    //pos.h = packet.ReadSInt16();

        //                    //text = packet.ReadPString(128, 1);

        //                    throw new NotImplementedException(nameof(DrawCmdTypes.DC_Text));
        //                }

        //                break;
        //            case DrawCmdTypes.DC_Shape:
        //                {
        //                    //penSize = packet.ReadSInt16();
        //                    //red = (byte)packet.ReadSInt16().SwapShort();
        //                    //green = (byte)packet.ReadSInt16().SwapShort();
        //                    //blue = (byte)packet.ReadSInt16().SwapShort();

        //                    //pos = new();
        //                    //pos.v = packet.ReadSInt16();
        //                    //pos.h = packet.ReadSInt16();

        //                    // TODO:

        //                    throw new NotImplementedException(nameof(DrawCmdTypes.DC_Shape));
        //                }

        //                break;
        //        }
        //}

        //public byte[] Serialize(params object[] values)
        //{
        //    using (var packet = new Packet())
        //    {
        //        packet.WriteInt16(nextOfst);
        //        packet.WriteInt16(0); //reserved
        //        packet.WriteInt16(DrawCmd);
        //        packet.WriteInt16(cmdLength);
        //        packet.WriteInt16(dataOfst);
        //        packet.WriteBytes(data, cmdLength);

        //        return packet.GetData();
        //    }
        //}
    }
}