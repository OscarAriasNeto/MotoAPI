using MotoAPI.Models;

namespace MotoAPI.Services
{
    public interface IClienteService
    {
        Task<(IEnumerable<Cliente> Items, int TotalItems)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Cliente?> GetTrackedByIdAsync(int id, CancellationToken cancellationToken);

        Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken);

        Task<Cliente> CreateAsync(Cliente cliente, CancellationToken cancellationToken);

        Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken);

        Task DeleteAsync(Cliente cliente, CancellationToken cancellationToken);
    }
}
