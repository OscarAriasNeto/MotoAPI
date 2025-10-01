using Microsoft.EntityFrameworkCore;
using MotoAPI.Models;

namespace MotoAPI.Data
{
    public class MotoDbContext : DbContext
    {
        public MotoDbContext(DbContextOptions<MotoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Moto> Motos => Set<Moto>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Estado)
                    .HasConversion<string>()
                    .HasMaxLength(20);
                entity.Property(m => m.ValorDiaria)
                    .HasPrecision(18, 2);
                entity.HasIndex(m => m.Placa)
                    .IsUnique();
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nome)
                    .HasMaxLength(150)
                    .IsRequired();
                entity.Property(c => c.Email)
                    .HasMaxLength(200)
                    .IsRequired();
                entity.HasIndex(c => c.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);
                entity.Property(p => p.ValorTotal)
                    .HasPrecision(18, 2);

                entity.HasOne(p => p.Cliente)
                    .WithMany(c => c.Pedidos)
                    .HasForeignKey(p => p.ClienteId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Moto)
                    .WithMany(m => m.Pedidos)
                    .HasForeignKey(p => p.MotoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
