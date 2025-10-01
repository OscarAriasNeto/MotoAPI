using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class UpdateMotoRequestExample : IExamplesProvider<UpdateMotoDto>
    {
        public UpdateMotoDto GetExamples() => new()
        {
            Modelo = "Honda CB 500X Adventure",
            AnoFabricacao = 2025,
            Placa = "DEF5678",
            ValorDiaria = 209.90m,
            Estado = EstadoMoto.Manutencao
        };
    }
}
