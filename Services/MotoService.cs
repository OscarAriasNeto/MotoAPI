using Microsoft.EntityFrameworkCore;
using MotoAPI.Data;
using MotoAPI.Models;

namespace MotoAPI.Services
{
    public class MotoService : IMotoService
    {
        private readonly MotoDbContext _context;

        public MotoService(MotoDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Moto> Items, int TotalItems)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Motos.AsNoTracking().OrderBy(m => m.Id);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }

        public Task<Moto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Motos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public Task<Moto?> GetTrackedByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Motos.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public Task<bool> ExistsWithPlacaAsync(string placa, CancellationToken cancellationToken)
        {
            return _context.Motos.AnyAsync(m => m.Placa == placa, cancellationToken);
        }

        public async Task<Moto> CreateAsync(Moto moto, CancellationToken cancellationToken)
        {
            _context.Motos.Add(moto);
            await _context.SaveChangesAsync(cancellationToken);
            return moto;
        }

        public async Task UpdateAsync(Moto moto, CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Moto moto, CancellationToken cancellationToken)
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
