using ThePalace.Common.Desktop.Entities.Ribbon;
using ThePalace.Common.Desktop.Factories;
using Timer = System.Timers.Timer;

namespace ThePalace.Client.Desktop.Entities.Core;

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    private volatile ToolStripItem _item1;
    private volatile ItemBase _item2;
    private volatile Timer _timer = new(250);

    public CustomToolStripRenderer()
    {
        _timer.Elapsed += (sender, e) =>
        {
            if (_item1 != null &&
                _item2?.HoverFrames != null)
                try
                {
                    _item1.Image = _item2.NextFrame();
                }
                catch
                {
                    _timer.Stop();

                    _item1 = null;
                    _item2 = null;
                }
        };
    }

    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
        base.OnRenderButtonBackground(e);

        var ribbonItem = SettingsManager.Ribbon[e.Item.Name];
        if (ribbonItem?.HoverFrames != null)
        {
            if (!e.Item.Selected)
            {
                _timer.Stop();

                _item1 = null;
                _item2 = null;
            }
            else
            {
                _item1 = e.Item;
                _item2 = ribbonItem;
                _item2.ResetFrames();

                _timer.Start();
            }
        }
    }
}