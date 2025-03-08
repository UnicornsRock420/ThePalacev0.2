using System.Windows.Forms.Design.Behavior;

namespace ThePalace.Common.Desktop.Entities.UI;

public delegate EventHandler Event(object sender, EventArgs e);

public delegate MouseEventHandler MouseEvent(object sender, MouseEventArgs e);

public delegate ScrollEventHandler ScrollEvent(object sender, ScrollEventArgs e);

public delegate BehaviorDragDropEventHandler DragDropEvent(object sender, BehaviorDragDropEventArgs e);

public class ControlCfg
{
    public Color BackColor;

    public BorderStyle BorderStyle;
    public EventHandler Click;
    public EventHandler DblClick;
    public DragEventHandler DragEnter;
    public EventHandler DragLeave;
    public DragEventHandler DragOver;
    public Color ForeColor;
    public Padding Margin;
    public int MaxLength;
    public MouseEventHandler MouseDown;
    public MouseEventHandler MouseExit;
    public MouseEventHandler MouseHover;
    public Point MouseLocation;
    public MouseEventHandler MouseMove;
    public MouseEventHandler MouseUp;

    public bool Multiline;
    public Padding Padding;

    public EventHandler Refresh;
    public ScrollEventHandler Scroll;
    public Size Size;
    public int TabIndex;
    public bool TabStop = false;
    public string? Title;

    public bool UseVisualStyleBackColor = true;
    public int Value;
    public bool Visible = true;
    public Point WindowLocation;
}