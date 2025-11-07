using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotoAPI.Data;
using MotoAPI.Models;

namespace MotoAPI.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MotoDbContext>));
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<MotoDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MotoApiIntegrationTests");
                });

                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<MotoDbContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                SeedDatabase(context);
            });
        }

        private static void SeedDatabase(MotoDbContext context)
        {
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
            });

            var cliente = new Cliente
            {
                Nome = "Maria Silva",
                Email = "maria.silva@email.com"
            };
            context.Clientes.Add(cliente);
            context.SaveChanges();

            context.Pedidos.Add(new Pedido
            {
                ClienteId = cliente.Id,
                MotoId = context.Motos.First().Id,
                DataRetirada = DateTime.UtcNow.Date,
                DataDevolucao = DateTime.UtcNow.Date.AddDays(2),
                ValorTotal = 199.80m,
                Status = StatusPedido.Reservado
            });

            context.SaveChanges();
        }
    }
}
