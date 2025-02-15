namespace ThePalace.Core.Interfaces.EventsBus
{
    public interface IEventHandler
    {
        Task<object?> Handle(object? sender, IEventParams @event);
    }

    public interface IEventHandler<in TEventArgs> : IEventHandler
        where TEventArgs : IEventParams
    {
    }
}