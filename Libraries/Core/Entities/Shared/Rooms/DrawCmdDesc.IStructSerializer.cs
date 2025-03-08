using System.Drawing;
using ThePalace.Core.Entities.Core;
using ThePalace.Core.Entities.Network.Shared.Network;
using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Data;
using AttributeExts = ThePalace.Core.Exts.AttributeExts;
using Point = ThePalace.Core.Entities.Shared.Types.Point;

namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class DrawCmdDesc : RawStream, IStructSerializer
{
    private static readonly int CONST_INT_SIZEOF_MSG_Header = AttributeExts.GetByteSize<MSG_Header>();
    private static readonly int CONST_INT_SIZEOF_POINT = AttributeExts.GetByteSize<Point>();

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        DrawCmdInfo.NextOfst = reader.ReadInt16();
        DrawCmdInfo.Reserved = reader.ReadInt16();
        DrawCmdInfo.DrawCmd = reader.ReadInt16();
        DrawCmdInfo.CmdLength = reader.ReadUInt16();
        DrawCmdInfo.DataOfst = reader.ReadInt16();

        reader.Position = DrawCmdInfo.DataOfst + CONST_INT_SIZEOF_MSG_Header;

        switch (Type)
        {
            case DrawCmdTypes.DC_Path:
            {
                PenSize = reader.ReadInt16();

                var nbrPoints = reader.ReadInt16();

                Red = (byte)reader.ReadInt16().SwapShort();
                Green = (byte)reader.ReadInt16().SwapShort();
                Blue = (byte)reader.ReadInt16().SwapShort();

                var vAxis = reader.ReadInt16();
                var hAxis = reader.ReadInt16();
                Pos = new Point(hAxis, vAxis);

                Points = [];
                while (Points.Count < nbrPoints &&
                       Length >= CONST_INT_SIZEOF_POINT)
                {
                    vAxis = reader.ReadInt16();
                    hAxis = reader.ReadInt16();
                    var p = new Point(vAxis, hAxis);

                    Points.Add(p);
                }
            }

                break;
            case DrawCmdTypes.DC_Ellipse:
            {
                PenSize = reader.ReadInt16();

                Red = (byte)reader.ReadInt16().SwapShort();
                Green = (byte)reader.ReadInt16().SwapShort();
                Blue = (byte)reader.ReadInt16().SwapShort();

                Rect = new Rectangle();
                Rect.X = reader.ReadInt16();
                Rect.Y = reader.ReadInt16();
                Rect.Width = reader.ReadInt16();
                Rect.Height = reader.ReadInt16();

                throw new NotImplementedException(nameof(DrawCmdTypes.DC_Ellipse));
            }

                break;
            case DrawCmdTypes.DC_Text:
            {
                PenSize = reader.ReadInt16();

                Red = (byte)reader.ReadInt16().SwapShort();
                Green = (byte)reader.ReadInt16().SwapShort();
                Blue = (byte)reader.ReadInt16().SwapShort();

                var vAxis = reader.ReadInt16();
                var hAxis = reader.ReadInt16();
                Pos = new Point(hAxis, vAxis);

                Text = ReadPString(128, 1);

                throw new NotImplementedException(nameof(DrawCmdTypes.DC_Text));
            }

                break;
            case DrawCmdTypes.DC_Shape:
            {
                PenSize = reader.ReadInt16();

                Red = (byte)reader.ReadInt16().SwapShort();
                Green = (byte)reader.ReadInt16().SwapShort();
                Blue = (byte)reader.ReadInt16().SwapShort();

                var vAxis = reader.ReadInt16();
                var hAxis = reader.ReadInt16();
                Pos = new Point(hAxis, vAxis);

                // TODO:

                throw new NotImplementedException(nameof(DrawCmdTypes.DC_Shape));
            }

                break;
        }
    }

    public void Serialize(Stream writer, SerializerOptions opts = SerializerOptions.None)
    {
        writer.WriteInt16(DrawCmdInfo.NextOfst);
        writer.WriteInt16(DrawCmdInfo.Reserved);
        writer.WriteInt16(DrawCmdInfo.DrawCmd);
        writer.WriteUInt16(DrawCmdInfo.CmdLength);
        writer.WriteInt16(DrawCmdInfo.DataOfst);
        writer.Write(Data, 0, DrawCmdInfo.CmdLength);
    }
}