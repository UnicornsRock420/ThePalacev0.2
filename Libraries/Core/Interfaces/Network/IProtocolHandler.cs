using ThePalace.Core.Entities.Events;

namespace ThePalace.Core.Interfaces.Network
{
    public interface IProtocolHandler
    {
        Task<object?> Handle(ProtocolEventArgs eventArgs);
    }

    public interface IProtocolHandler<TRequest> : IProtocolHandler
        where TRequest : IProtocol
    {
        Task<object?> Handle(ProtocolEventArgs eventArgs);
    }
}