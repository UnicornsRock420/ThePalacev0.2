using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.EventArgs;

public class ScriptEventParams : EventParams
{
    public int EventType;
    public object ScriptTag;
    public System.EventArgs EventArgs;
    public IProtocol? Msg;
}