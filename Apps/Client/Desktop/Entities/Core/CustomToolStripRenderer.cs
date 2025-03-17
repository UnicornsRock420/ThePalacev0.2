using Lib.Common.Desktop.Entities.Ribbon;
using Lib.Common.Desktop.Interfaces;
using ThePalace.Client.Desktop.Interfaces;
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
            if (_item1 != null)
                try
                {
                    switch (_item2)
                    {
                        case StandardItem _standardItem:
                            _item1.Image = _standardItem.Icon.NextFrame();

                            break;

                        case BooleanItem _booleanItem:
                            _item1.Image = (false ? _booleanItem.OnHoverIcon : _booleanItem.OffHoverIcon).NextFrame();

                            break;
                    }
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
        if (ribbonItem == null) return;

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

            switch (_item2)
            {
                case StandardItem _standardItem:
                    _standardItem.Icon.ResetFrames();
                    
                    break;
                case BooleanItem _booleanItem:
                    if (_booleanItem.State)
                        _booleanItem.OnHoverIcon.ResetFrames();
                    else
                        _booleanItem.OffHoverIcon.ResetFrames();
                    
                    break;
            }

            _timer.Start();
        }
    }
}