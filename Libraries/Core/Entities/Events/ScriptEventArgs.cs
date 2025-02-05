using ThePalace.Core.Enums;
using ThePalace.Core.Interfaces.Data;

namespace ThePalace.Core.Entities.Events
{
    public class ScriptEventArgs : EventArgs
    {
        public IptEventTypes EventType;
        public IStruct Packet;
        public object ScriptState;
    }
}