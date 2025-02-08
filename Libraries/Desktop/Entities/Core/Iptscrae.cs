using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using ThePalace.Core.Enums.Palace;
using Timer = System.Timers.Timer;

namespace ThePalace.Common.Desktop.Entities.Core
{
    using IptAlarms = List<IptAlarm>;
    using IptAtomList = List<IptVariable>;
    using IptEvents = ConcurrentDictionary<IptEventTypes, List<IptVariable>>;
    using IptVariables = ConcurrentDictionary<string, IptMetaVariable>;

    public delegate void IptCommandFnc(IptTracking iptTracking, int recursionDepth);
    public delegate IptVariable IptOperatorFnc(IptVariable register1, IptVariable register2);

    public partial class IptAlarm
    {
        public DateTime Created { get; private set; }
        public int Delay { get; private set; }
        public IptAtomList Value { get; private set; }

        public IptAlarm(IptAtomList value, int delay)
        {
            Created = DateTime.Now;
            Delay = delay;
            Value = value;
        }
    }

    public partial class IptVariable
    {
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
                else _value = value;
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
        public IptAtomList Stack { get; private set; } = new();
        public IptAlarms Alarms { get; private set; } = new();
        public IptEvents Events { get; private set; } = new();
        public IptVariables Variables { get; private set; } = new();

        public IptTracking() { }

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
}