using Microsoft.AspNetCore.Mvc;
using MotoAPI.Common;
using MotoAPI.DTOs;
using MotoAPI.Models;
using MotoAPI.Services;
using MotoAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace MotoAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    [Produces("application/json")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly LinkGenerator _linkGenerator;

        private string CurrentVersion => HttpContext?.GetRequestedApiVersion()?.ToString() ?? "1.0";

        public ClientesController(IClienteService clienteService, LinkGenerator linkGenerator)
        {
            _clienteService = clienteService;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Lista clientes com paginação.
        /// </summary>
        [HttpGet(Name = "GetClientes")]
        [ProducesResponseType(typeof(PagedResponse<ResourceDto<ClienteResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClientePagedResponseExample))]
        public async Task<ActionResult<PagedResponse<ResourceDto<ClienteResponseDto>>>> GetAsync([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var (items, total) = await _clienteService.GetPagedAsync(pagination.Page, pagination.PageSize, cancellationToken);
            var metadata = new PaginationMetadata(pagination.Page, pagination.PageSize, total);

            if (metadata.IsPageOutOfRange)
            {
                return NotFound(new { message = $"Página {pagination.Page} não disponível. Total de páginas: {metadata.TotalPages}." });
            }

            var resources = items
                .Select(ClienteResponseDto.FromEntity)
                .Select(BuildClienteResource)
                .ToList();

            var response = PagedResponseBuilder.Build(resources, total, pagination, _linkGenerator, HttpContext, "GetClientes");
            return Ok(response);
        }

        /// <summary>
        /// Obtém detalhes de um cliente.
        /// </summary>
        [HttpGet("{id:int}", Name = "GetClienteById")]
        [ProducesResponseType(typeof(ResourceDto<ClienteResponseDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ClienteResourceExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResourceDto<ClienteResponseDto>>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var cliente = await _clienteService.GetByIdAsync(id, cancellationToken);
            if (cliente is null)
            {
                return NotFound();
            }

            var resource = BuildClienteResource(ClienteResponseDto.FromEntity(cliente));
            return Ok(resource);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        [HttpPost(Name = "CreateCliente")]
        [ProducesResponseType(typeof(ResourceDto<ClienteResponseDto>), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(CreateClienteDto), typeof(CreateClienteRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ClienteResourceExample))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ResourceDto<ClienteResponseDto>>> PostAsync([FromBody] CreateClienteDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (await _clienteService.ExistsWithEmailAsync(dto.Email, cancellationToken))
            {
                return Conflict(new { message = $"Email {dto.Email} já cadastrado." });
            }

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Email = dto.Email
            };

            var created = await _clienteService.CreateAsync(cliente, cancellationToken);
            var resource = BuildClienteResource(ClienteResponseDto.FromEntity(created));
            return CreatedAtRoute("GetClienteById", new { version = CurrentVersion, id = resource.Data.Id }, resource);
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerRequestExample(typeof(UpdateClienteDto), typeof(UpdateClienteRequestExample))]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateClienteDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var cliente = await _clienteService.GetTrackedByIdAsync(id, cancellationToken);
            if (cliente is null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && !dto.Email.Equals(cliente.Email, StringComparison.OrdinalIgnoreCase))
            {
                if (await _clienteService.ExistsWithEmailAsync(dto.Email, cancellationToken))
                {
                    return Conflict(new { message = $"Email {dto.Email} já cadastrado." });
                }

                cliente.Email = dto.Email;
            }

            cliente.Nome = dto.Nome ?? cliente.Nome;

            await _clienteService.UpdateAsync(cliente, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Remove um cliente.
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var cliente = await _clienteService.GetTrackedByIdAsync(id, cancellationToken);
            if (cliente is null)
            {
                return NotFound();
            }

            await _clienteService.DeleteAsync(cliente, cancellationToken);
            return NoContent();
        }

        private ResourceDto<ClienteResponseDto> BuildClienteResource(ClienteResponseDto cliente)
        {
            var resource = new ResourceDto<ClienteResponseDto>(cliente);

            var self = _linkGenerator.GetUriByName(HttpContext, "GetClienteById", new { version = CurrentVersion, id = cliente.Id });
            if (!string.IsNullOrWhiteSpace(self))
            {
                resource.Links.Add(new LinkDto(self, "self", HttpMethods.Get));
            }

            var update = _linkGenerator.GetUriByName(HttpContext, "UpdateCliente", new { version = CurrentVersion, id = cliente.Id });
            if (!string.IsNullOrWhiteSpace(update))
            {
                resource.Links.Add(new LinkDto(update, "update", HttpMethods.Put));
            }

            var delete = _linkGenerator.GetUriByName(HttpContext, "DeleteCliente", new { version = CurrentVersion, id = cliente.Id });
            if (!string.IsNullOrWhiteSpace(delete))
            {
                resource.Links.Add(new LinkDto(delete, "delete", HttpMethods.Delete));
            }

            return resource;
        }
    }
}
