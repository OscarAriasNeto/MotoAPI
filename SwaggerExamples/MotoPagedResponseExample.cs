using Microsoft.AspNetCore.Http;
using MotoAPI.Common;
using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class MotoPagedResponseExample : IExamplesProvider<PagedResponse<ResourceDto<MotoResponseDto>>>
    {
        public PagedResponse<ResourceDto<MotoResponseDto>> GetExamples()
        {
            var moto = new MotoResponseDto(1, "Honda CB 500X", 2024, "ABC1234", 189.90m, EstadoMoto.Pronta, "Verde");
            var resource = new ResourceDto<MotoResponseDto>(moto);
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos/1", "self", HttpMethods.Get));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos/1", "update", HttpMethods.Put));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos/1", "delete", HttpMethods.Delete));

            var response = new PagedResponse<ResourceDto<MotoResponseDto>>(new[] { resource }, new PaginationMetadata(1, 10, 1));
            response.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos?page=1&pageSize=10", "self", HttpMethods.Get));
            return response;
        }
    }
}
