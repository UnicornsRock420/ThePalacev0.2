using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Client.Desktop.Events;

public class ScriptEventArgs : EventArgs
{
    public IptEventTypes EventType;
    public IProtocol Msg;
    public object ScriptState;
}