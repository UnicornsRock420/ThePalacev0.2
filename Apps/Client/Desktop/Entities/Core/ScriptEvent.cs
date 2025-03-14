﻿using ThePalace.Core.Interfaces.Network;
using ThePalace.Scripting.Iptscrae.Enums;

namespace ThePalace.Client.Desktop.Entities.Core;

public class ScriptEvent : EventArgs
{
    public IptEventTypes EventType { get; set; }
    public IProtocol Packet { get; set; }
    public object? ScriptTag { get; set; } = null;
}