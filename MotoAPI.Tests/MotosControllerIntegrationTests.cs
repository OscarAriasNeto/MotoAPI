using System.Net.Http.Json;
using System.Text.Json;

namespace MotoAPI.Tests
{
    public class MotosControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public MotosControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAsync_ReturnsPagedResponseWithLinks()
        {
            var response = await _client.GetAsync("/api/v1/motos?page=1&pageSize=5");

            response.EnsureSuccessStatusCode();
            var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
            Assert.NotNull(document);
            var root = document!.RootElement;
            Assert.True(root.TryGetProperty("items", out var items));
            Assert.True(items.GetArrayLength() > 0);
            Assert.True(root.TryGetProperty("links", out var links));
            Assert.True(links.GetArrayLength() >= 1);
        }

        [Fact]
        public async Task GetById_ReturnsResourceWithHateoasLinks()
        {
            var response = await _client.GetAsync("/api/v1/motos/1");

            response.EnsureSuccessStatusCode();
            var document = await response.Content.ReadFromJsonAsync<JsonDocument>();
            Assert.NotNull(document);
            var root = document!.RootElement;
            Assert.True(root.TryGetProperty("data", out var data));
            Assert.Equal("Honda CG 160", data.GetProperty("modelo").GetString());
            Assert.True(root.TryGetProperty("links", out var links));
            Assert.True(links.GetArrayLength() >= 3);
        }
    }
}
