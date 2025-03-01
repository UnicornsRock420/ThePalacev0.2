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

public partial class IptAlarm
{
    public DateTime Created { get; internal set; }
    public int Delay { get; internal set; }
    public IptAtomList Value { get; internal set; }

    public IptAlarm(IptAtomList value, int delay)
    {
        Created = DateTime.UtcNow;
        Delay = delay;
        Value = value;
    }
}

public partial class IptVariable
{
    public IptVariable() { }
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

    public IptVariableTypes Type;
    public object Value;
}

public partial class IptMetaVariable
{
    public bool IsReadOnly = false;
    public bool IsSpecial = false;
    public bool IsGlobal = false;
    public int Depth = 0;

    private IptVariable _value;
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

public partial class IptOperator
{
    public IptOperatorFlags Flags;
    public IptOperatorFnc OpFnc;
}

public class IptTracking : IDisposable
{
    public static int TicksToMilliseconds(int ticks) => ticks / 6 * 100;

    public readonly Timer Timer = new Timer
    {
        Interval = 10, //IptTracking.TicksToMilliseconds(1)
    };

    public bool Return = false;
    public bool Break = false;
    public Match[] Grep;
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
}