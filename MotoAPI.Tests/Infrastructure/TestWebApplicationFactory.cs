using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoAPI.Data;
using MotoAPI.Models;

namespace MotoAPI.Tests.Infrastructure;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string ApiKeyHeaderName = "X-API-KEY";
    public const string ApiKey = "integration-test-key";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            var overrides = new Dictionary<string, string?>
            {
                ["ApiKey:Value"] = ApiKey
            };

            configurationBuilder.AddInMemoryCollection(overrides);
        });

        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MotoDbContext>();
            context.Database.EnsureCreated();

            if (!context.Motos.Any())
            {
                context.Motos.Add(new Moto
                {
                    Modelo = "CB 500X",
                    AnoFabricacao = 2023,
                    Placa = "INT1234",
                    ValorDiaria = 120m,
                    Estado = "Disponivel"
                });

                context.SaveChanges();
            }
        });
    }
}
