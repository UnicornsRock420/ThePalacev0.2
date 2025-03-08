using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Client.Desktop.Entities.Core;

public class ScriptEvent : EventArgs
{
    public IptEventTypes EventType { get; set; }
    public IProtocol Packet { get; set; }
    public object ScriptState { get; set; }
}