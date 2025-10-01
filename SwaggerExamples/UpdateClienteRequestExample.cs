using MotoAPI.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.SwaggerExamples
{
    public class UpdateClienteRequestExample : IExamplesProvider<UpdateClienteDto>
    {
        public UpdateClienteDto GetExamples() => new()
        {
            Nome = "Maria Souza",
            Email = "maria.souza@email.com"
        };
    }
}
