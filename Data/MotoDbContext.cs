using Microsoft.EntityFrameworkCore;
using MotoAPI.Models;

namespace MotoAPI.Data
{
    public class MotoDbContext : DbContext
    {
        public MotoDbContext(DbContextOptions<MotoDbContext> options) : base(options) { }

        public DbSet<Moto> Motos => Set<Moto>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>(entity =>
            {
                entity.ToTable("Motos");
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedNever();
                entity.HasIndex(m => m.Placa).IsUnique();
                entity.Property(m => m.Estado).HasConversion<string>();
            });
        }
    }
}