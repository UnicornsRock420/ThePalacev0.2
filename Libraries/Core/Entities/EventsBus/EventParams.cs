using System.Runtime.Serialization;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Core.Entities.EventsBus;

public abstract class EventParams : System.EventArgs, IEventParams
{
    protected EventParams()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
    protected EventParams(
        Guid id,
        DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }

    [IgnoreDataMember]
    public Guid Id { get; protected set; }

    [IgnoreDataMember]
    public DateTime OccurredOn { get; protected set; }
}