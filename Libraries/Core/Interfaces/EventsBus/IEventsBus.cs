namespace Lib.Core.Interfaces.EventsBus;

public interface IEventsBus : IDisposable
{
    Task Publish(object? sender, IEventParams @event);

    void Subscribe(IEventHandler handler);
}

public interface IEventsBus<T> : IDisposable
    where T : IEventParams
{
    Task Publish(object? sender, T @event);

    void Subscribe(IEventHandler<T> handler);
}