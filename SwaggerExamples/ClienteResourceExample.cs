using Microsoft.AspNetCore.Http;
using MotoAPI.Common;
using MotoAPI.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class ClienteResourceExample : IExamplesProvider<ResourceDto<ClienteResponseDto>>
    {
        public ResourceDto<ClienteResponseDto> GetExamples()
        {
            var cliente = new ClienteResponseDto(1, "Maria Silva", "maria.silva@email.com", DateTime.UtcNow.Date.AddDays(-10));
            var resource = new ResourceDto<ClienteResponseDto>(cliente);
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes/1", "self", HttpMethods.Get));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes/1", "update", HttpMethods.Put));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes/1", "delete", HttpMethods.Delete));
            return resource;
        }
    }
}
