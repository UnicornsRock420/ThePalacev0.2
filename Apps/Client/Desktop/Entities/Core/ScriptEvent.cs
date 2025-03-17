using Lib.Core.Interfaces.Network;
using Mod.Scripting.Iptscrae.Enums;

namespace ThePalace.Client.Desktop.Entities.Core;

public class ScriptEvent : EventArgs
{
    public IptEventTypes EventType { get; set; }
    public IProtocol Msg { get; set; }
    public object? ScriptTag { get; set; } = null;
}