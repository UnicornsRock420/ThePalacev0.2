using System.Runtime.Serialization;
using Lib.Core.Interfaces.EventsBus;

namespace Lib.Core.Entities.EventsBus;

public class EventParams : System.EventArgs, IEventParams
{
    public EventParams()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public EventParams(
        Guid id,
        DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }

    [IgnoreDataMember] public Guid Id { get; }

    [IgnoreDataMember] public DateTime OccurredOn { get; }
}