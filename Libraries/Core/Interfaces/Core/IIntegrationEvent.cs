using MediatR;

namespace ThePalace.Core.Interfaces.Core
{
    public interface IIntegrationEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}