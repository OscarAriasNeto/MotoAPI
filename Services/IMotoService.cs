using MotoAPI.Models;

namespace MotoAPI.Services
{
    public interface IMotoService
    {
        Task<(IEnumerable<Moto> Items, int TotalItems)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);

        Task<Moto?> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Moto?> GetTrackedByIdAsync(int id, CancellationToken cancellationToken);

        Task<bool> ExistsWithPlacaAsync(string placa, CancellationToken cancellationToken);

        Task<Moto> CreateAsync(Moto moto, CancellationToken cancellationToken);

        Task UpdateAsync(Moto moto, CancellationToken cancellationToken);

        Task DeleteAsync(Moto moto, CancellationToken cancellationToken);
    }
}
