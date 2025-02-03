using ThePalace.Core.Attributes;

namespace ThePalace.Core.Enums
{
    [ByteSize(1)]
    public enum DrawCmdTypes : byte
    {
        DC_Path = 0,
        DC_Shape = 1,
        DC_Text = 2,
        DC_Detonate = 3,
        DC_Delete = 4,
        DC_Ellipse = 5,
    };
}