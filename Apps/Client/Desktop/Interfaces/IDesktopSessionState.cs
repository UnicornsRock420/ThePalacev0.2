using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Client.Interfaces;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;

namespace ThePalace.Client.Desktop.Interfaces;

public interface IDesktopSessionState<TDesktopApp> : IClientSessionState<TDesktopApp>, IUISessionState<TDesktopApp>
    where TDesktopApp : IDesktopApp
{
    IReadOnlyDictionary<LayerScreenTypes, ILayerScreen> UILayers { get; }

    bool Visible { get; set; }
    bool Enabled { get; set; }

    double Scale { get; set; }
    int ScreenWidth { get; set; }
    int ScreenHeight { get; set; }

    DateTime? LastActivity { get; set; }
    AssetSpec SelectedProp { get; set; }
    UserDesc SelectedUser { get; set; }
    HotspotDesc SelectedHotSpot { get; set; }

    HistoryManager History { get; }
    TabPage TabPage { get; set; }

    void RefreshScriptEvent(ScriptEvent scriptEvent);
    void RefreshRibbon();
    void RefreshUI();
    void RefreshScreen(params LayerScreenTypes[] layers);
    void LayerVisibility(bool visible, params LayerScreenTypes[] layers);
    void LayerOpacity(float opacity, params LayerScreenTypes[] layers);
}