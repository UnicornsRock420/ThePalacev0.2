namespace ThePalace.Core.Interfaces
{
    public interface IAsyncGenericService<TRequest, TWhere> : IAsyncService<TRequest, TWhere>
    {
        Task<object?> Create(TRequest request, CancellationToken cancellationToken);
        Task<object?> Get(TRequest request, TWhere? where, CancellationToken cancellationToken);
        Task<object[]?> GetAll(TRequest request, TWhere? where, CancellationToken cancellationToken);
        Task<object?> Update(TRequest request, TWhere? where, CancellationToken cancellationToken);
        Task<object?> Delete(TRequest request, TWhere? where, CancellationToken cancellationToken);
    }
}