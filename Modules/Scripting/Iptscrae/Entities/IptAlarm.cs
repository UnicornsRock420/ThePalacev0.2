namespace ThePalace.Scripting.Iptscrae.Entities;

using IptAtomList = List<IptVariable>;

public class IptAlarm
{
    public IptAlarm(IptAtomList atomList, int delay)
    {
        ArgumentNullException.ThrowIfNull(atomList, nameof(atomList));

        if (delay < 0) throw new IndexOutOfRangeException(nameof(delay));

        AtomList = atomList;
        Delay = TicksToMilliseconds<double>(delay);

        var now = DateTime.UtcNow;
        
        Created = now;
        Expires = now.AddMilliseconds(Delay); 
    }

    public IptAtomList AtomList { get; internal set; }
    public double Delay { get; internal set; }
    
    public DateTime Created { get; internal set; }
    public DateTime Expires { get; internal set; }
    public bool IsElapsed => DateTime.UtcNow > Expires;

    public static TResult TicksToMilliseconds<TResult>(object value)
        where TResult : struct
    {
        return (TResult)(object)((long)value / 6 * 100);
    }
}