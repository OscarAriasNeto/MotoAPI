using MotoAPI.Models;

namespace MotoAPI.Services
{
    public interface IPedidoService
    {
        Task<(IEnumerable<Pedido> Items, int TotalItems)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<Pedido?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Pedido?> GetTrackedByIdAsync(int id, CancellationToken cancellationToken);

        Task<Pedido> CreateAsync(Pedido pedido, CancellationToken cancellationToken);

        Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken);

        Task DeleteAsync(Pedido pedido, CancellationToken cancellationToken);
    }
}
