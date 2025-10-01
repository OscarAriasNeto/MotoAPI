using Microsoft.AspNetCore.Http;
using MotoAPI.Common;
using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class PedidoResourceExample : IExamplesProvider<ResourceDto<PedidoResponseDto>>
    {
        public ResourceDto<PedidoResponseDto> GetExamples()
        {
            var pedido = new PedidoResponseDto(1, 1, 2, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(3), 559.70m, StatusPedido.EmAndamento);
            var resource = new ResourceDto<PedidoResponseDto>(pedido);
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/pedidos/1", "self", HttpMethods.Get));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/pedidos/1", "update", HttpMethods.Put));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/pedidos/1", "delete", HttpMethods.Delete));
            return resource;
        }
    }
}
