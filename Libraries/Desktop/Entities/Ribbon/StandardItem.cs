using System.Drawing;

namespace ThePalace.Common.Desktop.Entities.Ribbon;

public abstract class StandardItem : ItemBase
{
    public string Icon { get; set; }
    public Bitmap Image { get; set; }
}