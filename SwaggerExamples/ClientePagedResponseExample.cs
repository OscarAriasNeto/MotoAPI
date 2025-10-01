using Microsoft.AspNetCore.Http;
using MotoAPI.Common;
using MotoAPI.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class ClientePagedResponseExample : IExamplesProvider<PagedResponse<ResourceDto<ClienteResponseDto>>>
    {
        public PagedResponse<ResourceDto<ClienteResponseDto>> GetExamples()
        {
            var cliente = new ClienteResponseDto(1, "Maria Silva", "maria.silva@email.com", DateTime.UtcNow.Date.AddDays(-10));
            var resource = new ResourceDto<ClienteResponseDto>(cliente);
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes/1", "self", HttpMethods.Get));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes/1", "update", HttpMethods.Put));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes/1", "delete", HttpMethods.Delete));

            var response = new PagedResponse<ResourceDto<ClienteResponseDto>>(new[] { resource }, new PaginationMetadata(1, 10, 1));
            response.Links.Add(new LinkDto("https://localhost:5001/api/v1/clientes?page=1&pageSize=10", "self", HttpMethods.Get));
            return response;
        }
    }
}
