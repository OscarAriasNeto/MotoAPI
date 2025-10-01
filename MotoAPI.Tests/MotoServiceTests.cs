using Microsoft.EntityFrameworkCore;
using MotoAPI.Data;
using MotoAPI.Models;
using MotoAPI.Services;

namespace MotoAPI.Tests
{
    public class MotoServiceTests
    {
        private static MotoDbContext BuildContext()
        {
            var options = new DbContextOptionsBuilder<MotoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new MotoDbContext(options);
            context.Motos.AddRange(new Moto
            {
                Modelo = "Honda CG 160",
                AnoFabricacao = 2023,
                Placa = "ABC1234",
                ValorDiaria = 89.90m,
                Estado = EstadoMoto.Pronta
            },
            new Moto
            {
                Modelo = "Yamaha MT-07",
                AnoFabricacao = 2024,
                Placa = "DEF5678",
                ValorDiaria = 199.90m,
                Estado = EstadoMoto.Pronta
            },
            new Moto
            {
                Modelo = "BMW GS 850",
                AnoFabricacao = 2024,
                Placa = "GHI9012",
                ValorDiaria = 349.90m,
                Estado = EstadoMoto.Manutencao
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetPagedAsync_ReturnsPagedItems()
        {
            using var context = BuildContext();
            var service = new MotoService(context);

            var (items, total) = await service.GetPagedAsync(page: 1, pageSize: 2, CancellationToken.None);

            Assert.Equal(3, total);
            Assert.Equal(2, items.Count());
            Assert.Equal("Honda CG 160", items.First().Modelo);
        }

        [Fact]
        public async Task ExistsWithPlacaAsync_ReturnsTrueForExistingPlate()
        {
            using var context = BuildContext();
            var service = new MotoService(context);

            var exists = await service.ExistsWithPlacaAsync("DEF5678", CancellationToken.None);

            Assert.True(exists);
        }

        [Fact]
        public async Task CreateAsync_PersistsEntity()
        {
            using var context = BuildContext();
            var service = new MotoService(context);

            var moto = new Moto
            {
                Modelo = "Ducati Monster",
                AnoFabricacao = 2024,
                Placa = "JKL3456",
                ValorDiaria = 299.90m,
                Estado = EstadoMoto.Pronta
            };

            await service.CreateAsync(moto, CancellationToken.None);

            Assert.Equal(4, context.Motos.Count());
        }
    }
}
