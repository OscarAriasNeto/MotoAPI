using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MotoAPI.Middleware;

public class ApiKeyMiddleware
{
    private const string HeaderName = "X-API-KEY";
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAnonymousPath(context.Request.Path) || IsSwaggerPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(HeaderName, out var extractedKey))
        {
            await WriteErrorAsync(context, StatusCodes.Status401Unauthorized, "Chave de API ausente no cabeçalho X-API-KEY.");
            return;
        }

        var configuredKey = _configuration.GetValue<string>("ApiKey:Value");
        if (string.IsNullOrWhiteSpace(configuredKey) || !string.Equals(configuredKey, extractedKey, StringComparison.Ordinal))
        {
            await WriteErrorAsync(context, StatusCodes.Status403Forbidden, "Chave de API inválida.");
            return;
        }

        await _next(context);
    }

    private static bool IsAnonymousPath(PathString path)
        => path.HasValue && path.Value is not null && path.Value.Equals("/health", StringComparison.OrdinalIgnoreCase);

    private static bool IsSwaggerPath(PathString path)
        => path.HasValue && path.Value is not null &&
           (path.Value.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            path.Value.StartsWith("/favicon.ico", StringComparison.OrdinalIgnoreCase));

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(payload);
    }
}
