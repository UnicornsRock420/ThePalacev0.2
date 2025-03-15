using ThePalace.Core.Interfaces.Network;
using ThePalace.Scripting.Iptscrae.Enums;

namespace ThePalace.Client.Desktop.Events;

public class ScriptEventArgs : EventArgs
{
    public IptEventTypes EventType;
    public IProtocol Msg;
    public object ScriptState;
}