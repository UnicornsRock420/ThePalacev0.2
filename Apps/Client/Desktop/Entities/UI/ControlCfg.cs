using System.Windows.Forms.Design.Behavior;

namespace ThePalace.Client.Desktop.Entities.Threads.UI
{
    public delegate EventHandler Event(object sender, EventArgs e);

    public class ControlCfg
    {
        public int TabIndex;
        public string Text;
        public Size Size;
        public Padding Margin;
        public Padding Padding;
        public Point Location;
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
        public MouseEventHandler MouseHover;
        public MouseEventHandler MouseExit;
        public ScrollEventHandler Scroll;
        public BehaviorDragDropEventHandler DragDrop;
    }
}