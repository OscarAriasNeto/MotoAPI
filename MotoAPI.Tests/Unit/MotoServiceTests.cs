using Microsoft.EntityFrameworkCore;
using MotoAPI.Data;
using MotoAPI.Models;
using MotoAPI.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MotoAPI.Tests.Unit
{
    public class MotoServiceTests
    {
        [Fact]
        public async Task GetPagedAsync_ReturnsRequestedPageWithTotalCount()
        {
            using var context = CreateContext();
            await SeedMotosAsync(context);
            var service = new MotoService(context);

            var (items, total) = await service.GetPagedAsync(page: 2, pageSize: 1, CancellationToken.None);

            Assert.Equal(3, total);
            var moto = Assert.Single(items);
            Assert.Equal("Modelo B", moto.Modelo);
        }

        [Fact]
        public async Task ExistsWithPlacaAsync_ReturnsTrueWhenMotoExists()
        {
            using var context = CreateContext();
            var service = new MotoService(context);

            context.Motos.Add(new Moto
            {
                Modelo = "Modelo Único",
                AnoFabricacao = 2021,
                Placa = "ABC1234",
                ValorDiaria = 100m
            });
            await context.SaveChangesAsync();

            var exists = await service.ExistsWithPlacaAsync("ABC1234", CancellationToken.None);
            var notExists = await service.ExistsWithPlacaAsync("XYZ9999", CancellationToken.None);

            Assert.True(exists);
            Assert.False(notExists);
        }

        private static MotoDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<MotoDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MotoDbContext(options);
        }

        private static async Task SeedMotosAsync(MotoDbContext context)
        {
            context.Motos.AddRange(
                new Moto { Modelo = "Modelo A", AnoFabricacao = 2020, Placa = "AAA0001", ValorDiaria = 80m },
                new Moto { Modelo = "Modelo B", AnoFabricacao = 2022, Placa = "BBB0002", ValorDiaria = 120m },
                new Moto { Modelo = "Modelo C", AnoFabricacao = 2019, Placa = "CCC0003", ValorDiaria = 60m }
            );

            await context.SaveChangesAsync();
        }
    }
}
