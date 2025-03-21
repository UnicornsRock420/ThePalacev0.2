using Lib.Core.Enums;
using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.EventArgs;

public class ScriptEventParams : EventParams
{
    public ScriptEventTypes EventType;
    public IProtocol Msg;
    public object ScriptState;
}