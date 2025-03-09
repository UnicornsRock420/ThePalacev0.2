using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ThePalace.Core.Enums;
using Timer = System.Timers.Timer;

namespace ThePalace.Core.Entities.Scripting;

using IptAlarms = List<IptAlarm>;
using IptAtomList = List<IptVariable>;
using IptEvents = ConcurrentDictionary<IptEventTypes, List<IptVariable>>;
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
        Interval = IptAlarm.TicksToMilliseconds<double>(1),
        Enabled = false,
    };

    public IptTrackingFlags Flags { get; internal set; } = IptTrackingFlags.None;
    public IptAtomList Stack { get; internal set; } = new();
    public IptAlarms Alarms { get; internal set; } = new();
    public IptEvents Events { get; internal set; } = new();
    public IptVariables Variables { get; internal set; } = new();
    public Match[] Grep { get; internal set; } = null;
}