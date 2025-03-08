using System.Runtime.Serialization;
using ThePalace.Core.Interfaces.EventsBus;

namespace ThePalace.Core.Entities.EventsBus;

public partial class EventParams : System.EventArgs, IEventParams
{
    public EventParams()
    {
    }

    public EventParams(
        Guid id,
        DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }

    [IgnoreDataMember] public Guid Id { get; } = Guid.NewGuid();

    [IgnoreDataMember] public DateTime OccurredOn { get; } = DateTime.UtcNow;
}