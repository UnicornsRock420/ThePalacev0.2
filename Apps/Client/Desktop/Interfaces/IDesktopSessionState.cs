using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Client.Desktop.Interfaces;

public interface IDesktopSessionState : IClientSessionState, IUISessionState
{
    IReadOnlyDictionary<string, IDisposable> UIControls { get; }
    IReadOnlyDictionary<LayerScreenTypes, ILayerScreen> UILayers { get; }

    bool Visible { get; set; }
    double Scale { get; set; }
    int ScreenWidth { get; set; }
    int ScreenHeight { get; set; }

    AssetSpec SelectedProp { get; set; }
    UserDesc SelectedUser { get; set; }
    HotspotDesc SelectedHotSpot { get; set; }

    HistoryManager History { get; }
    TabPage TabPage { get; set; }

    void RefreshScriptEvent(ScriptEvent  scriptEvent);
    void RefreshRibbon();
    void RefreshUI();
    void RefreshScreen(params LayerScreenTypes[] layers);
    void LayerVisibility(bool visible, params LayerScreenTypes[] layers);
    void LayerOpacity(float opacity, params LayerScreenTypes[] layers);

    FormBase GetForm(string? friendlyName = null);
    T GetForm<T>(string? friendlyName = null) where T : FormBase;
    void RegisterForm(string friendlyName, FormBase form);
    void RegisterForm<T>(string friendlyName, T form) where T : FormBase;
    void UnregisterForm(string friendlyName, FormBase form);
    void UnregisterForm<T>(string friendlyName, T form) where T : FormBase;
    Control GetControl(string? friendlyName = null);
    T GetControl<T>(string? friendlyName = null) where T : Control;
    void RegisterControl(string friendlyName, Control control);
    void RegisterControl<T>(string friendlyName, T control) where T : Control;
    void RegisterControl(string friendlyName, IDisposable control);
    void UnregisterControl(string friendlyName, Control control);
    void UnregisterControl<T>(string friendlyName, T control) where T : Control;
    void UnregisterControl(string friendlyName, IDisposable control);
}