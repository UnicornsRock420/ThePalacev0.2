using System.Windows.Forms.Design.Behavior;

namespace ThePalace.Common.Desktop.Entities.UI;

public delegate EventHandler Event(object sender, EventArgs e);
public delegate MouseEventHandler MouseEvent(object sender, MouseEventArgs e);
public delegate ScrollEventHandler ScrollEvent(object sender, ScrollEventArgs e);
public delegate BehaviorDragDropEventHandler DragDropEvent(object sender, BehaviorDragDropEventArgs e);

public class ControlCfg
{
    public int TabIndex;
    public string? Title;
    public Size Size;
    public Padding Margin;
    public Padding Padding;
    public Point WindowLocation;
    public Point MouseLocation;
    public Color BackColor;
    public Color ForeColor;
    public bool Visible = true;
    public bool TabStop = false;

    public BorderStyle BorderStyle;

    public bool UseVisualStyleBackColor = true;

    public bool Multiline;
    public int MaxLength;
    public int Value;

    public EventHandler Refresh;
    public EventHandler Click;
    public EventHandler DblClick;
    public MouseEventHandler MouseMove;
    public MouseEventHandler MouseUp;
    public MouseEventHandler MouseDown;
    public MouseEventHandler MouseHover;
    public MouseEventHandler MouseExit;
    public ScrollEventHandler Scroll;
    public DragEventHandler DragEnter;
    public EventHandler DragLeave;
    public DragEventHandler DragOver;
}