using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class UpdatePedidoRequestExample : IExamplesProvider<UpdatePedidoDto>
    {
        public UpdatePedidoDto GetExamples() => new()
        {
            DataRetirada = DateTime.UtcNow.Date.AddDays(1),
            DataDevolucao = DateTime.UtcNow.Date.AddDays(5),
            ValorTotal = 699.90m,
            Status = StatusPedido.EmAndamento
        };
    }
}
