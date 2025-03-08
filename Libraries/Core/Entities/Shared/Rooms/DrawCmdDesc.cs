namespace ThePalace.Core.Entities.Shared.Rooms;

public partial class DrawCmdDesc
{
    public DrawCmdRec DrawCmdInfo;

    public DrawCmdDesc()
    {
        DrawCmdInfo = new DrawCmdRec();
    }
}