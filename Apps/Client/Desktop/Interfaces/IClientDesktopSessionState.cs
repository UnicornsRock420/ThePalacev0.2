using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Entities.Core;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Client.Interfaces;
using ThePalace.Common.Desktop.Entities.Ribbon;
using ThePalace.Common.Desktop.Interfaces;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.Types;
using ThePalace.Core.Entities.Shared.Users;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Client.Desktop.Interfaces;

public interface IClientDesktopSessionState<TApp> : IClientSessionState<TApp>, IDesktopSessionState<TApp>
    where TApp : IApp
{
    IReadOnlyDictionary<LayerScreenTypes, ILayerScreen> UILayers { get; }

    AssetSpec SelectedProp { get; set; }
    UserDesc SelectedUser { get; set; }
    HotspotDesc SelectedHotSpot { get; set; }

    HistoryManager History { get; }
    TabPage TabPage { get; set; }
    ConcurrentDictionary<Guid, ItemBase> Ribbon { get; }

    void LayerVisibility(bool visible, params LayerScreenTypes[] layers);
    void LayerOpacity(float opacity, params LayerScreenTypes[] layers);
    void RefreshScreen(params LayerScreenTypes[] layers);
    void RefreshUI();
    void RefreshScriptEvent(ScriptEvent scriptEvent);
}