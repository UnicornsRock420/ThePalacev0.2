using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces;

namespace ThePalace.Core.Entities.Events
{
    public class ScriptEventArgs : EventArgs
    {
        public IptEventTypes EventType;
        public IProtocol Packet;
        public object ScriptState;
    }
}