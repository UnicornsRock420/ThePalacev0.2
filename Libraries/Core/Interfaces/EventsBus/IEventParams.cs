using MediatR;

namespace Lib.Core.Interfaces.EventsBus;

public interface IEventParams : INotification
{
    Guid Id { get; }

    DateTime OccurredOn { get; }
}