using Microsoft.EntityFrameworkCore;
using MotoAPI.Data;
using MotoAPI.Models;

namespace MotoAPI.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly MotoDbContext _context;

        public PedidoService(MotoDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Pedido> Items, int TotalItems)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Pedidos.AsNoTracking().OrderBy(p => p.Id);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }

        public Task<Pedido?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public Task<Pedido?> GetTrackedByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Pedido> CreateAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync(cancellationToken);
            return pedido;
        }

        public async Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Pedido pedido, CancellationToken cancellationToken)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
