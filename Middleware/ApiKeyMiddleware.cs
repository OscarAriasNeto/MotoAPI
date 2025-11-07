using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MotoAPI.Options;

namespace MotoAPI.Middleware;

public class ApiKeyMiddleware
{
    private const string HeaderName = "X-API-KEY";
    private readonly RequestDelegate _next;
    private readonly string? _configuredKey;

    public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeyOptions> options)
    {
        _next = next;
        _configuredKey = options.Value.Value;
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

        if (string.IsNullOrWhiteSpace(_configuredKey))
        {
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Chave de API não configurada.");
            return;
        }

        if (!string.Equals(_configuredKey, extractedKey, StringComparison.Ordinal))
        {
            await WriteErrorAsync(context, StatusCodes.Status403Forbidden, "Chave de API inválida.");
            return;
        }

        await _next(context);
    }

    private static bool IsAnonymousPath(PathString path)
        => path.HasValue && path.StartsWithSegments("/health");

    private static bool IsSwaggerPath(PathString path)
        => path.HasValue && (path.StartsWithSegments("/swagger") || path.StartsWithSegments("/favicon.ico"));

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(payload);
    }
}
