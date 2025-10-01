using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class CreateMotoRequestExample : IExamplesProvider<CreateMotoDto>
    {
        public CreateMotoDto GetExamples() => new()
        {
            Modelo = "Honda CB 500X",
            AnoFabricacao = 2024,
            Placa = "ABC1234",
            ValorDiaria = 189.90m,
            Estado = EstadoMoto.Pronta
        };
    }
}
