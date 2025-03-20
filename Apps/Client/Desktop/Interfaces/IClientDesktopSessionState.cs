using System.Collections.Concurrent;
using Lib.Common.Client.Interfaces;
using Lib.Common.Desktop.Entities.Ribbon;
using Lib.Common.Desktop.Interfaces;
using Lib.Core.Entities.Scripting;
using Lib.Core.Entities.Shared.Rooms;
using Lib.Core.Entities.Shared.Types;
using Lib.Core.Entities.Shared.Users;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;

namespace ThePalace.Client.Desktop.Interfaces;

public interface IClientDesktopSessionState : IClientSessionState, IDesktopSessionState
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