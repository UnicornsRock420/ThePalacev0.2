using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ThePalace.Core.Enums;
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
    public IptAlarm(IptAtomList atomList, int delay)
    {
        Created = DateTime.UtcNow;
        AtomList = atomList;
        Delay = delay;
    }

    public DateTime Created { get; internal set; }

    public IptAtomList AtomList { get; internal set; }
    public double Delay { get; internal set; }
}

public class IptVariable
{
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

    public IptVariableTypes Type { get; internal set; }
    public object Value { get; internal set; }

    public T GetValue<T>()
    {
        return (T)Value;
    }
}

public class IptMetaVariable
{
    public IptMetaVariable()
    {
    }

    public IptMetaVariable(int depth, IptVariable variable, IptMetaVariableFlags flags = IptMetaVariableFlags.None)
    {
        Depth = depth;
        Flags = flags;
        Variable = variable;
    }

    public IptMetaVariable(IptMetaVariable src) : this(src.Depth, src.Variable, src.Flags)
    {
    }

    public int Depth = 0;
    public IptMetaVariableFlags Flags = IptMetaVariableFlags.None;
    public bool IsGlobal => (Flags & IptMetaVariableFlags.IsGlobal) == IptMetaVariableFlags.IsGlobal;
    public bool IsReadOnly => (Flags & IptMetaVariableFlags.IsReadOnly) == IptMetaVariableFlags.IsReadOnly;
    public bool IsSpecial => (Flags & IptMetaVariableFlags.IsSpecial) == IptMetaVariableFlags.IsSpecial;

    private IptVariable _variable;
    public IptVariable Variable
    {
        get => _variable;
        set
        {
            if (!IsReadOnly) return;

            _variable = value;
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

    private Timer _timer;
    public Timer Timer => _timer ??= new()
    {
        Interval = TicksToMilliseconds<double>(1),
        Enabled = false,
    };

    public IptTrackingFlags Flags { get; internal set; } = IptTrackingFlags.None;
    public IptAtomList Stack { get; internal set; } = new();
    public IptAlarms Alarms { get; internal set; } = new();
    public IptEvents Events { get; internal set; } = new();
    public IptVariables Variables { get; internal set; } = new();
    public Match[] Grep { get; internal set; } = null;

    public static TResult TicksToMilliseconds<TResult>(object value)
        where TResult : struct
    {
        return (TResult)(object)((long)value / 6 * 100);
    }
}