namespace ThePalace.Core.Entities.Shared.Rooms
{
    public partial class DrawCmdDesc
    {
        public DrawCmdDesc()
        {
            DrawCmdInfo = new();
        }

        public DrawCmdRec DrawCmdInfo;
    }
}