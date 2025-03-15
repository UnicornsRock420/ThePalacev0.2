using ThePalace.Client.Desktop.Interfaces;
using ThePalace.Common.Desktop.Entities.Ribbon;
using ThePalace.Common.Desktop.Interfaces;
using Timer = System.Timers.Timer;

namespace ThePalace.Client.Desktop.Entities.Core;

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    private volatile ToolStripItem _item1;
    private volatile ItemBase _item2;
    private volatile Timer _timer = new(250);

    private IClientDesktopSessionState<IDesktopApp> _sessionState;

    protected CustomToolStripRenderer()
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

    public CustomToolStripRenderer(IClientDesktopSessionState<IDesktopApp> sessionState) : this()
    {
        ArgumentNullException.ThrowIfNull(sessionState, nameof(sessionState));

        _sessionState = sessionState;
    }

    protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
    {
        base.OnRenderButtonBackground(e);

        var key = e.Item?.Tag as Guid?;
        if (key == null) return;

        var ribbonItem = _sessionState.Ribbon.GetValue(key.Value);
        if (ribbonItem?.HoverFrames == null) return;

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