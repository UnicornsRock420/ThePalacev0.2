namespace ThePalace.Core.Interfaces.Core
{
    public interface IIntegrationEventHandler
    {
        Task<object?> Handle(object? sender, IIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IIntegrationEvent
    {
    }
}