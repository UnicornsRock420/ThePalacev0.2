namespace ThePalace.Client.Desktop.Entities.Threads.UI
{
    public partial class FormCfg : ControlCfg
    {
        public SizeF AutoScaleDimensions;
        public AutoScaleMode AutoScaleMode;
        public FormStartPosition StartPosition;
        public FormWindowState WindowState;
        public bool Focus;

        public EventHandler Activated;
        public EventHandler Load;
        public EventHandler Shown;
        public EventHandler GotFocus;
        public MouseEventHandler MouseMove;
        public FormClosedEventHandler FormClosed;
    }
}