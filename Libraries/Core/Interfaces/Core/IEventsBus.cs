namespace ThePalace.Core.Interfaces.Core
{
    public interface IEventsBus : IDisposable
    {
        Task Publish(object? sender, IIntegrationEvent @event);

        void Subscribe(IIntegrationEventHandler handler);
    }

    public interface IEventsBus<T> : IDisposable
        where T : IIntegrationEvent
    {
        Task Publish(object? sender, T @event);

        void Subscribe(IIntegrationEventHandler<T> handler);
    }
}