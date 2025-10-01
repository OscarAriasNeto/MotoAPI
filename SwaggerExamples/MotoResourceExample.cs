using Microsoft.AspNetCore.Http;
using MotoAPI.Common;
using MotoAPI.DTOs;
using MotoAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class MotoResourceExample : IExamplesProvider<ResourceDto<MotoResponseDto>>
    {
        public ResourceDto<MotoResponseDto> GetExamples()
        {
            var moto = new MotoResponseDto(1, "Honda CB 500X", 2024, "ABC1234", 189.90m, EstadoMoto.Pronta, "Verde");
            var resource = new ResourceDto<MotoResponseDto>(moto);
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos/1", "self", HttpMethods.Get));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos/1", "update", HttpMethods.Put));
            resource.Links.Add(new LinkDto("https://localhost:5001/api/v1/motos/1", "delete", HttpMethods.Delete));
            return resource;
        }
    }
}
