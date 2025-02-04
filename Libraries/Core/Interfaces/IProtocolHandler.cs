namespace ThePalace.Core.Interfaces
{
    public interface IProtocolHandler<TRequest>
        where TRequest : IProtocol
    {
        Task<object?> Handle(int? sourceID, int refNum, TRequest request, CancellationToken cancellationToken);
    }
}