using System.Drawing;

namespace ThePalace.Common.Desktop.Entities.Ribbon
{
    public abstract class BooleanItem : ItemBase
    {
        public bool State { get; set; } = false;

        public string OnIcon { get; set; }
        public Bitmap OnImage { get; set; }
        public string OffIcon { get; set; }
        public Bitmap OffImage { get; set; }
    }
}
