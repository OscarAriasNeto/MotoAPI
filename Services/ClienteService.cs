using Microsoft.EntityFrameworkCore;
using MotoAPI.Data;
using MotoAPI.Models;

namespace MotoAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly MotoDbContext _context;

        public ClienteService(MotoDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Cliente> Items, int TotalItems)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Clientes.AsNoTracking().OrderBy(c => c.Id);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }

        public Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public Task<Cliente?> GetTrackedByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken)
        {
            return _context.Clientes.AnyAsync(c => c.Email == email, cancellationToken);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync(cancellationToken);
            return cliente;
        }

        public async Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
