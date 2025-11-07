using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MotoAPI.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public const string ApiKey = "integration-test-key";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ApiKey"] = ApiKey
                });
            });
        }
    }

    public class BasicIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public BasicIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("X-API-KEY", CustomWebApplicationFactory.ApiKey);
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsHealthyStatus()
        {
            var response = await _client.GetAsync("/api/v1/health");

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal("Healthy", json.GetProperty("status").GetString());
        }

        [Fact]
        public async Task PredictionsEndpoint_ReturnsBadRequestWhenModelIsMissing()
        {
            var response = await _client.PostAsJsonAsync(
                "/api/v1/predicoes/motos/valor-diaria",
                new { AnoFabricacao = 2020, Quilometragem = 5000f });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PredictionsEndpoint_ReturnsPredictionWhenPayloadIsValid()
        {
            var response = await _client.PostAsJsonAsync(
                "/api/v1/predicoes/motos/valor-diaria",
                new { Modelo = "Honda CG 160", AnoFabricacao = 2020, Quilometragem = 5000f });

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            Assert.True(json.TryGetProperty("valorDiaria", out var value));
            Assert.True(value.GetDouble() > 0);
        }
    }
}
