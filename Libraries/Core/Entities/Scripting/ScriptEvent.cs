using Lib.Core.Interfaces.Network;

namespace Lib.Core.Entities.Scripting;

public class ScriptEvent : EventArgs
{
    public int EventType { get; set; }
    public IProtocol Msg { get; set; }
    public object? ScriptTag { get; set; } = null;
}