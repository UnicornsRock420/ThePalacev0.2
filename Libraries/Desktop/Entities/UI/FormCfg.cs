namespace Lib.Common.Desktop.Entities.UI;

public class FormCfg : ControlCfg
{
    public string Name { get; set; }
    public EventHandler Activated { get; set; }
    public SizeF AutoScaleDimensions { get; set; }
    public AutoScaleMode AutoScaleMode { get; set; }
    public bool Focus { get; set; }
    public FormClosedEventHandler FormClosed { get; set; }
    public EventHandler GotFocus { get; set; }
    public EventHandler Load { get; set; }
    public MouseEventHandler MouseMove { get; set; }
    public EventHandler Shown { get; set; }
    public FormStartPosition StartPosition { get; set; }
    public FormWindowState WindowState { get; set; }
}