using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MotoAPI.Data;
using MotoAPI.Services;
using MotoAPI.SwaggerExamples;
using System;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MotoAPI",
        Description = "API para gestão de motos, clientes e pedidos com HATEOAS e paginação"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.EnableAnnotations();
    options.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateMotoRequestExample>();

builder.Services.AddDbContext<MotoDbContext>(options =>
    options.UseInMemoryDatabase("MotoDb"));

builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoAPI v1");
    options.DocumentTitle = "MotoAPI - Documentação";
});

app.UseHttpsRedirection();

var apiKey = builder.Configuration["ApiKey"];
const string apiKeyHeaderName = "X-API-KEY";

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    if (string.IsNullOrEmpty(apiKey) || !context.Request.Headers.TryGetValue(apiKeyHeaderName, out var extractedKey) || !string.Equals(extractedKey, apiKey, StringComparison.Ordinal))
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("API Key inválida ou ausente.");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
