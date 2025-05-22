using Microsoft.EntityFrameworkCore;
using MotoAPI.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext (removida duplicação)
builder.Services.AddDbContext<MotoDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Configuração dos Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo

    {

        Title = "Moto Manager API",
        Version = "v1",
        Description = "API para gestão de motos e estados operacionais",
        Contact = new OpenApiContact
        {
            Name = "Suporte Técnico",
            Email = "suporte@motomanager.com",
            Url = new Uri("https://motomanager.com/suporte")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Configuração da documentação XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
        c.EnableAnnotations();
    }
    else
    {
        Console.WriteLine($"AVISO: Arquivo de documentação {xmlFile} não encontrado!");
    }
});

var app = builder.Build();

// Habilitar Swagger em todos os ambientes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moto API v1");
    c.RoutePrefix = string.Empty; // Acessar na raiz do domínio
    c.DocumentTitle = "Documentação da API de Motos";
});

// Pipeline de execução
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Executar aplicação
app.Run();