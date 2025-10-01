using MotoAPI.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class CreateClienteRequestExample : IExamplesProvider<CreateClienteDto>
    {
        public CreateClienteDto GetExamples() => new()
        {
            Nome = "Maria Silva",
            Email = "maria.silva@email.com"
        };
    }
}
