using Microsoft.EntityFrameworkCore;
using MotoAPI.Models;

namespace MotoAPI.Data // Namespace ESSENCIAL
{
    public class MotoDbContext : DbContext
    {
        public MotoDbContext(DbContextOptions<MotoDbContext> options)
            : base(options) { }

        public DbSet<Moto> Motos => Set<Moto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>(entity =>
            {
                entity.ToTable("MOTOS");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Estado)
                      .HasConversion<string>()
                      .HasMaxLength(20);
                entity.HasIndex(m => m.Placa)
                      .IsUnique();
            });
        }
    }
}