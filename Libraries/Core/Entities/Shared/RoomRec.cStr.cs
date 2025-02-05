using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomRec : RawData
    {
        public RoomRec()
        {
            _data = new();

            HotSpots = new();
            Pictures = new();
            DrawCmds = new();
            LooseProps = new();
        }
        public RoomRec(uint8[]? data = null)
        {
            _data = new(data);

            HotSpots = new();
            Pictures = new();
            DrawCmds = new();
            LooseProps = new();
        }

        public override void Dispose()
        {
            _data?.Clear();
            _data = null;

            HotSpots?.Clear();
            HotSpots = null;

            Pictures?.Clear();
            Pictures = null;

            DrawCmds?.Clear();
            DrawCmds = null;

            LooseProps?.Clear();
            LooseProps = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}