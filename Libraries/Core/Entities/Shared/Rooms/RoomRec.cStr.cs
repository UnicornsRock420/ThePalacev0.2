using ThePalace.Core.Entities.Core;
using uint8 = System.Byte;

namespace ThePalace.Core.Entities.Shared
{
    public partial class RoomRec : RawStream
    {
        public RoomRec()
        {
            this._stream = new();

            this.HotSpots = new();
            this.Pictures = new();
            this.DrawCmds = new();
            this.LooseProps = new();
        }
        public RoomRec(uint8[]? data = null)
        {
            this._stream = new(data);

            this.HotSpots = new();
            this.Pictures = new();
            this.DrawCmds = new();
            this.LooseProps = new();
        }

        ~RoomRec() => this.Dispose();

        public override void Dispose()
        {
            this.HotSpots?.Clear();
            this.HotSpots = null;

            this.Pictures?.Clear();
            this.Pictures = null;

            this.DrawCmds?.Clear();
            this.DrawCmds = null;

            this.LooseProps?.Clear();
            this.LooseProps = null;

            base.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}