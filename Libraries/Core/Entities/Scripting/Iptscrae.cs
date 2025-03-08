using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ThePalace.Core.Enums.Palace;
using Timer = System.Timers.Timer;

namespace ThePalace.Core.Entities.Scripting;

using IptAlarms = List<IptAlarm>;
using IptAtomList = List<IptVariable>;
using IptEvents = ConcurrentDictionary<IptEventTypes, List<IptVariable>>;
using IptVariables = ConcurrentDictionary<string, IptMetaVariable>;

public delegate void IptCommandFnc(IptTracking iptTracking, int recursionDepth);

public delegate IptVariable IptOperatorFnc(IptVariable register1, IptVariable register2);

public class IptAlarm
{
    public IptAlarm(IptAtomList value, int delay)
    {
        Created = DateTime.UtcNow;
        Delay = delay;
        Value = value;
    }

    public DateTime Created { get; internal set; }
    public int Delay { get; internal set; }
    public IptAtomList Value { get; internal set; }
}

public class IptVariable
{
    public IptVariableTypes Type;
    public object Value;

    public IptVariable()
    {
    }

    public IptVariable(IptVariableTypes type)
    {
        Type = type;
    }

    public IptVariable(
        IptVariableTypes type,
        object value)
    {
        Type = type;
        Value = value;
    }
}

public class IptMetaVariable
{
    private IptVariable _value;
    public int Depth = 0;
    public bool IsGlobal = false;
    public bool IsReadOnly = false;
    public bool IsSpecial = false;

    public IptVariable Value
    {
        get => _value;
        set
        {
            if (IsReadOnly) return;

            _value = value;
        }
    }
}

public class IptOperator
{
    public IptOperatorFlags Flags;
    public IptOperatorFnc OpFnc;
}

public class IptTracking : IDisposable
{
    public readonly Timer Timer = new()
    {
        Interval = 10 //IptTracking.TicksToMilliseconds(1)
    };

    public bool Break = false;
    public Match[] Grep;

    public bool Return = false;
    public IptAtomList Stack { get; internal set; } = new();
    public IptAlarms Alarms { get; internal set; } = new();
    public IptEvents Events { get; internal set; } = new();
    public IptVariables Variables { get; internal set; } = new();

    public void Dispose()
    {
        Events?.Clear();
        Events = null;
        Variables?.Clear();
        Variables = null;
        Stack?.Clear();
        Stack = null;
        Grep = null;
    }

    public static int TicksToMilliseconds(int ticks)
    {
        return ticks / 6 * 100;
    }
}