using MediatR;

namespace ThePalace.Core.Interfaces.Core
{
    public interface IEventParams : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}