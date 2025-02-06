using ThePalace.Core.Enums.Palace;
using ThePalace.Core.Interfaces.Network;

namespace ThePalace.Core.Entities.Events
{
    public class ScriptEventArgs : EventArgs
    {
        public IptEventTypes EventType;
        public IProtocol Msg;
        public object ScriptState;
    }
}