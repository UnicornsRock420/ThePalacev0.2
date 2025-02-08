using System.Runtime.Serialization;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Core
{
    public abstract class EventParams : EventArgs, IEventParams
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
}