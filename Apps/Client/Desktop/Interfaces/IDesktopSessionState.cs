using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;

namespace ThePalace.Client.Desktop.Interfaces;

public interface IDesktopSessionState : IUISessionState
{
    IReadOnlyDictionary<string, IDisposable> UIControls { get; }
    IReadOnlyDictionary<ScreenLayerTypes, IScreenLayer> UILayers { get; }

    bool Visible { get; set; }
    double Scale { get; set; }
    int ScreenWidth { get; set; }
    int ScreenHeight { get; set; }

    AssetSpec SelectedProp { get; set; }
    UserDesc SelectedUser { get; set; }
    HotspotDesc SelectedHotSpot { get; set; }

    HistoryManager History { get; }
    TabPage TabPage { get; set; }

    void Refresh(ScriptEvent  scriptEvent);
    void RefreshRibbon();
    void RefreshUI();
    void RefreshScreen(params ScreenLayerTypes[] layers);
    void LayerVisibility(bool visible, params ScreenLayerTypes[] layers);
    void LayerOpacity(float opacity, params ScreenLayerTypes[] layers);

    FormBase GetForm(string? friendlyName = null);
    T GetForm<T>(string? friendlyName = null) where T : FormBase;
    void RegisterForm(string friendlyName, FormBase form);
    void UnregisterForm(string friendlyName, FormBase form);
    Control GetControl(string? friendlyName = null);
    void RegisterControl(string friendlyName, Control control);
    void RegisterControl(string friendlyName, IDisposable control);
    void UnregisterForm(string friendlyName, Control control);
    void UnregisterForm(string friendlyName, IDisposable control);
}