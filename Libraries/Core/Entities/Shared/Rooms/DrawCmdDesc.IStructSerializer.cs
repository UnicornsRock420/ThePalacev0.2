using System.Drawing;
using Lib.Core.Entities.Network.Shared.Network;
using Lib.Core.Enums;
using Lib.Core.Exts;
using Lib.Core.Interfaces.Data;
using AttributeExts = Lib.Core.Exts.AttributeExts;
using Point = Lib.Core.Entities.Shared.Types.Point;

namespace Lib.Core.Entities.Shared.Rooms;

public partial class DrawCmdDesc : IStructSerializer
{
    private static readonly int CONST_INT_SIZEOF_MSG_Header = AttributeExts.GetByteSize<MSG_Header>();
    private static readonly int CONST_INT_SIZEOF_POINT = AttributeExts.GetByteSize<Point>();

    public void Deserialize(Stream reader, SerializerOptions opts = SerializerOptions.None)
    {
        NextOfst = reader.ReadInt16();
        Reserved = reader.ReadInt16();
        DrawCmd = reader.ReadInt16();
        CmdLength = reader.ReadUInt16();
        DataOfst = reader.ReadInt16();

        reader.Position = DataOfst + CONST_INT_SIZEOF_MSG_Header;

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
                       reader.Length >= CONST_INT_SIZEOF_POINT)
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

                Text = reader.ReadPString(128, 1);

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
        writer.WriteInt16(NextOfst);
        writer.WriteInt16(Reserved);
        writer.WriteInt16(DrawCmd);
        writer.WriteUInt16((ushort)(Data?.Length ?? 0));
        writer.WriteInt16(DataOfst);
        if ((Data?.Length ?? 0) > 0)
            writer.Write(Data, 0, Data.Length);
    }
}