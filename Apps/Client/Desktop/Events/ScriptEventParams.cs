using Lib.Core.Entities.EventsBus;
using Lib.Core.Interfaces.Network;
using Mod.Scripting.Iptscrae.Enums;

namespace ThePalace.Client.Desktop.Events;

public class ScriptEventParams : EventParams
{
    public IptEventTypes EventType;
    public IProtocol Msg;
    public object ScriptState;
}