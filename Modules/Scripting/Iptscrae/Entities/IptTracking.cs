﻿using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Lib.Core.Enums;
using Mod.Scripting.Iptscrae.Enums;
using Timer = System.Timers.Timer;

namespace Mod.Scripting.Iptscrae.Entities;

using IptAlarms = List<IptAlarm>;
using IptAtomList = List<IptVariable>;
using IptEvents = ConcurrentDictionary<ScriptEventTypes, List<IptVariable>>;
using IptVariables = ConcurrentDictionary<string, IptMetaVariable>;

public class IptTracking : IDisposable
{
    ~IptTracking() => Dispose();

    public void Dispose()
    {
        _timer?.Dispose();
        _timer = null;

        Stack?.Clear();
        Stack = null;

        Alarms?.Clear();
        Alarms = null;

        Events?.Clear();
        Events = null;

        Variables?.Clear();
        Variables = null;

        Grep = null;

        GC.SuppressFinalize(this);
    }

    private Timer _timer = null;

    public Timer Timer => _timer ??= new()
    {
        Interval = IptAlarm.TicksToMs<double>(1),
        Enabled = false,
    };

    public IptTrackingFlags Flags { get; internal set; } = IptTrackingFlags.None;
    public IptAtomList Stack { get; internal set; } = [];
    public IptAlarms Alarms { get; internal set; } = [];
    public IptEvents Events { get; internal set; } = new();
    public IptVariables Variables { get; internal set; } = new();
    public Match[] Grep { get; internal set; } = null;
}