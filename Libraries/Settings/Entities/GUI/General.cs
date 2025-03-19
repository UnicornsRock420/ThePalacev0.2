using Lib.Settings.Attributes;

namespace Lib.Settings.Entities.GUI;

[XPath("$.GUI:{}")]
public class General
{
    public string InterpolationMode  { get; set; }
    public string PixelOffsetMode  { get; set; }
    public string SmoothingMode  { get; set; }
    public bool SysTrayIcon  { get; set; }
}