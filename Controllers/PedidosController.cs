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
    [Route("api/v{version:apiVersion}/pedidos")]
    [Produces("application/json")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;
        private readonly IMotoService _motoService;
        private readonly LinkGenerator _linkGenerator;

        private string CurrentVersion => HttpContext?.GetRequestedApiVersion()?.ToString() ?? "1.0";

        public PedidosController(IPedidoService pedidoService, IClienteService clienteService, IMotoService motoService, LinkGenerator linkGenerator)
        {
            _pedidoService = pedidoService;
            _clienteService = clienteService;
            _motoService = motoService;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Lista pedidos com paginação.
        /// </summary>
        [HttpGet(Name = "GetPedidos")]
        [ProducesResponseType(typeof(PagedResponse<ResourceDto<PedidoResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PedidoPagedResponseExample))]
        public async Task<ActionResult<PagedResponse<ResourceDto<PedidoResponseDto>>>> GetAsync([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var (items, total) = await _pedidoService.GetPagedAsync(pagination.Page, pagination.PageSize, cancellationToken);
            var metadata = new PaginationMetadata(pagination.Page, pagination.PageSize, total);

            if (metadata.IsPageOutOfRange)
            {
                return NotFound(new { message = $"Página {pagination.Page} não disponível. Total de páginas: {metadata.TotalPages}." });
            }

            var resources = items
                .Select(PedidoResponseDto.FromEntity)
                .Select(BuildPedidoResource)
                .ToList();

            var response = PagedResponseBuilder.Build(resources, total, pagination, _linkGenerator, HttpContext, "GetPedidos");
            return Ok(response);
        }

        /// <summary>
        /// Obtém detalhes de um pedido.
        /// </summary>
        [HttpGet("{id:int}", Name = "GetPedidoById")]
        [ProducesResponseType(typeof(ResourceDto<PedidoResponseDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PedidoResourceExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResourceDto<PedidoResponseDto>>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoService.GetByIdAsync(id, cancellationToken);
            if (pedido is null)
            {
                return NotFound();
            }

            var resource = BuildPedidoResource(PedidoResponseDto.FromEntity(pedido));
            return Ok(resource);
        }

        /// <summary>
        /// Cria um novo pedido.
        /// </summary>
        [HttpPost(Name = "CreatePedido")]
        [ProducesResponseType(typeof(ResourceDto<PedidoResponseDto>), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(CreatePedidoDto), typeof(CreatePedidoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(PedidoResourceExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ResourceDto<PedidoResponseDto>>> PostAsync([FromBody] CreatePedidoDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (dto.DataDevolucao.HasValue && dto.DataDevolucao < dto.DataRetirada)
            {
                ModelState.AddModelError(nameof(dto.DataDevolucao), "Data de devolução não pode ser anterior à retirada.");
                return UnprocessableEntity(ModelState);
            }

            var cliente = await _clienteService.GetByIdAsync(dto.ClienteId, cancellationToken);
            if (cliente is null)
            {
                return NotFound(new { message = $"Cliente {dto.ClienteId} não encontrado." });
            }

            var moto = await _motoService.GetByIdAsync(dto.MotoId, cancellationToken);
            if (moto is null)
            {
                return NotFound(new { message = $"Moto {dto.MotoId} não encontrada." });
            }

            var pedido = new Pedido
            {
                ClienteId = dto.ClienteId,
                MotoId = dto.MotoId,
                DataRetirada = dto.DataRetirada,
                DataDevolucao = dto.DataDevolucao,
                ValorTotal = dto.ValorTotal,
                Status = dto.Status
            };

            var created = await _pedidoService.CreateAsync(pedido, cancellationToken);
            var resource = BuildPedidoResource(PedidoResponseDto.FromEntity(created));
            return CreatedAtRoute("GetPedidoById", new { version = CurrentVersion, id = resource.Data.Id }, resource);
        }

        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdatePedido")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerRequestExample(typeof(UpdatePedidoDto), typeof(UpdatePedidoRequestExample))]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdatePedidoDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var pedido = await _pedidoService.GetTrackedByIdAsync(id, cancellationToken);
            if (pedido is null)
            {
                return NotFound();
            }

            if (dto.DataRetirada.HasValue)
            {
                pedido.DataRetirada = dto.DataRetirada.Value;
            }

            if (dto.DataDevolucao.HasValue)
            {
                if (dto.DataDevolucao < pedido.DataRetirada)
                {
                    ModelState.AddModelError(nameof(dto.DataDevolucao), "Data de devolução não pode ser anterior à retirada.");
                    return UnprocessableEntity(ModelState);
                }

                pedido.DataDevolucao = dto.DataDevolucao;
            }

            if (dto.ValorTotal.HasValue)
            {
                pedido.ValorTotal = dto.ValorTotal.Value;
            }

            if (dto.Status.HasValue)
            {
                pedido.Status = dto.Status.Value;
            }

            await _pedidoService.UpdateAsync(pedido, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Remove um pedido.
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeletePedido")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoService.GetTrackedByIdAsync(id, cancellationToken);
            if (pedido is null)
            {
                return NotFound();
            }

            await _pedidoService.DeleteAsync(pedido, cancellationToken);
            return NoContent();
        }

        private ResourceDto<PedidoResponseDto> BuildPedidoResource(PedidoResponseDto pedido)
        {
            var resource = new ResourceDto<PedidoResponseDto>(pedido);

            var self = _linkGenerator.GetUriByName(HttpContext, "GetPedidoById", new { version = CurrentVersion, id = pedido.Id });
            if (!string.IsNullOrWhiteSpace(self))
            {
                resource.Links.Add(new LinkDto(self, "self", HttpMethods.Get));
            }

            var update = _linkGenerator.GetUriByName(HttpContext, "UpdatePedido", new { version = CurrentVersion, id = pedido.Id });
            if (!string.IsNullOrWhiteSpace(update))
            {
                resource.Links.Add(new LinkDto(update, "update", HttpMethods.Put));
            }

            var delete = _linkGenerator.GetUriByName(HttpContext, "DeletePedido", new { version = CurrentVersion, id = pedido.Id });
            if (!string.IsNullOrWhiteSpace(delete))
            {
                resource.Links.Add(new LinkDto(delete, "delete", HttpMethods.Delete));
            }

            return resource;
        }
    }
}
