using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class CreatePedidoRequestExample : IExamplesProvider<CreatePedidoDto>
    {
        public CreatePedidoDto GetExamples() => new()
        {
            ClienteId = 1,
            MotoId = 2,
            DataRetirada = DateTime.UtcNow.Date.AddDays(1),
            DataDevolucao = DateTime.UtcNow.Date.AddDays(4),
            ValorTotal = 559.70m,
            Status = StatusPedido.Reservado
        };
    }
}
