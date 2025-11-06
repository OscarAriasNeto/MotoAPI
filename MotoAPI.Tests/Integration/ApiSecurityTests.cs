using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using MotoAPI.Tests.Infrastructure;
using Xunit;

namespace MotoAPI.Tests.Integration;

public class ApiSecurityTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiSecurityTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
    }

    [Fact]
    public async Task GetMotos_WithoutApiKey_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/v1/motos");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMotos_WithApiKey_ReturnsSuccess()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/motos");
        request.Headers.Add(TestWebApplicationFactory.ApiKeyHeaderName, TestWebApplicationFactory.ApiKey);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
