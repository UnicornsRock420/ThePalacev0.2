using System.Runtime.Serialization;
using ThePalace.Core.Interfaces.Core;

namespace ThePalace.Core.Entities.Core
{
    public abstract class IntegrationEvent : EventArgs, IEventArgs, IIntegrationEvent
    {
        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
        protected IntegrationEvent(
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