using System.Collections.Concurrent;
using ThePalace.Client.Desktop.Entities.UI;
using ThePalace.Client.Desktop.Enums;
using ThePalace.Client.Desktop.Factories;
using ThePalace.Common.Desktop.Forms.Core;
using ThePalace.Core.Entities.Shared;
using ThePalace.Core.Entities.Shared.Rooms;
using ThePalace.Core.Entities.Shared.ServerInfo;
using ThePalace.Core.Entities.Shared.Types;

namespace ThePalace.Common.Desktop.Interfaces;

public interface IDesktopSessionState : IUISessionState
{
    IReadOnlyDictionary<string, IDisposable> UIControls { get; }
    IReadOnlyDictionary<ScreenLayers, ScreenLayer> UILayers { get; }

    bool Visible { get; set; }
    double Scale { get; set; }
    int ScreenWidth { get; set; }
    int ScreenHeight { get; set; }

    HistoryManager History { get; }
    TabPage TabPage { get; set; }

    RoomDesc RoomInfo { get; set; }
    ConcurrentDictionary<uint, UserDesc> RoomUsers { get; set; }

    string? MediaUrl { get; set; }
    string? ServerName { get; set; }
    int ServerPopulation { get; set; }
    List<ListRec> ServerRooms { get; set; }
    List<ListRec> ServerUsers { get; set; }

    AssetSpec SelectedProp { get; set; }
    UserDesc SelectedUser { get; set; }
    HotspotDesc SelectedHotSpot { get; set; }

    void RefreshRibbon();
    void RefreshUI();
    void RefreshScreen(params ScreenLayers[] layers);
    void LayerVisibility(bool visible, params ScreenLayers[] layers);
    void LayerOpacity(float opacity, params ScreenLayers[] layers);

    FormBase GetForm(string friendlyName);
    T GetForm<T>(string friendlyName) where T : FormBase;
    void RegisterForm(string friendlyName, FormBase form);
    void UnregisterForm(string friendlyName, FormBase form);
    Control GetControl(string friendlyName);
    void RegisterControl(string friendlyName, Control control);
    void RegisterControl(string friendlyName, IDisposable control);
    void UnregisterForm(string friendlyName, Control control);
    void UnregisterForm(string friendlyName, IDisposable control);
}