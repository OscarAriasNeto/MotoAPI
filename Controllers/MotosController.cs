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
    [Route("api/v{version:apiVersion}/motos")]
    [Produces("application/json")]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;
        private readonly LinkGenerator _linkGenerator;

        private string CurrentVersion => HttpContext?.GetRequestedApiVersion()?.ToString() ?? "1.0";

        public MotosController(IMotoService motoService, LinkGenerator linkGenerator)
        {
            _motoService = motoService;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Lista motocicletas com paginação.
        /// </summary>
        [HttpGet(Name = "GetMotos")]
        [ProducesResponseType(typeof(PagedResponse<ResourceDto<MotoResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoPagedResponseExample))]
        public async Task<ActionResult<PagedResponse<ResourceDto<MotoResponseDto>>>> GetAsync([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var (items, total) = await _motoService.GetPagedAsync(pagination.Page, pagination.PageSize, cancellationToken);
            var metadata = new PaginationMetadata(pagination.Page, pagination.PageSize, total);

            if (metadata.IsPageOutOfRange)
            {
                return NotFound(new { message = $"Página {pagination.Page} não disponível. Total de páginas: {metadata.TotalPages}." });
            }

            var resources = items
                .Select(MotoResponseDto.FromEntity)
                .Select(BuildMotoResource)
                .ToList();

            var response = PagedResponseBuilder.Build(resources, total, pagination, _linkGenerator, HttpContext, "GetMotos");
            return Ok(response);
        }

        /// <summary>
        /// Obtém detalhes de uma motocicleta.
        /// </summary>
        [HttpGet("{id:int}", Name = "GetMotoById")]
        [ProducesResponseType(typeof(ResourceDto<MotoResponseDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MotoResourceExample))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResourceDto<MotoResponseDto>>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var moto = await _motoService.GetByIdAsync(id, cancellationToken);
            if (moto is null)
            {
                return NotFound();
            }

            var resource = BuildMotoResource(MotoResponseDto.FromEntity(moto));
            return Ok(resource);
        }

        /// <summary>
        /// Cria uma nova motocicleta.
        /// </summary>
        [HttpPost(Name = "CreateMoto")]
        [ProducesResponseType(typeof(ResourceDto<MotoResponseDto>), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(CreateMotoDto), typeof(CreateMotoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(MotoResourceExample))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<ResourceDto<MotoResponseDto>>> PostAsync([FromBody] CreateMotoDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            if (await _motoService.ExistsWithPlacaAsync(dto.Placa, cancellationToken))
            {
                return Conflict(new { message = $"Placa {dto.Placa} já cadastrada." });
            }

            var moto = new Moto
            {
                Modelo = dto.Modelo,
                AnoFabricacao = dto.AnoFabricacao,
                Placa = dto.Placa,
                ValorDiaria = dto.ValorDiaria,
                Estado = dto.Estado
            };

            var created = await _motoService.CreateAsync(moto, cancellationToken);
            var resource = BuildMotoResource(MotoResponseDto.FromEntity(created));
            return CreatedAtRoute("GetMotoById", new { version = CurrentVersion, id = resource.Data.Id }, resource);
        }

        /// <summary>
        /// Atualiza uma motocicleta existente.
        /// </summary>
        [HttpPut("{id:int}", Name = "UpdateMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [SwaggerRequestExample(typeof(UpdateMotoDto), typeof(UpdateMotoRequestExample))]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateMotoDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var moto = await _motoService.GetTrackedByIdAsync(id, cancellationToken);
            if (moto is null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(dto.Placa) && !dto.Placa.Equals(moto.Placa, StringComparison.OrdinalIgnoreCase))
            {
                if (await _motoService.ExistsWithPlacaAsync(dto.Placa, cancellationToken))
                {
                    return Conflict(new { message = $"Placa {dto.Placa} já cadastrada." });
                }

                moto.Placa = dto.Placa;
            }

            moto.Modelo = dto.Modelo ?? moto.Modelo;
            moto.AnoFabricacao = dto.AnoFabricacao ?? moto.AnoFabricacao;
            moto.ValorDiaria = dto.ValorDiaria ?? moto.ValorDiaria;
            moto.Estado = dto.Estado ?? moto.Estado;

            await _motoService.UpdateAsync(moto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Remove uma motocicleta.
        /// </summary>
        [HttpDelete("{id:int}", Name = "DeleteMoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var moto = await _motoService.GetTrackedByIdAsync(id, cancellationToken);
            if (moto is null)
            {
                return NotFound();
            }

            await _motoService.DeleteAsync(moto, cancellationToken);
            return NoContent();
        }

        private ResourceDto<MotoResponseDto> BuildMotoResource(MotoResponseDto moto)
        {
            var resource = new ResourceDto<MotoResponseDto>(moto);

            var self = _linkGenerator.GetUriByName(HttpContext, "GetMotoById", new { version = CurrentVersion, id = moto.Id });
            if (!string.IsNullOrWhiteSpace(self))
            {
                resource.Links.Add(new LinkDto(self, "self", HttpMethods.Get));
            }

            var update = _linkGenerator.GetUriByName(HttpContext, "UpdateMoto", new { version = CurrentVersion, id = moto.Id });
            if (!string.IsNullOrWhiteSpace(update))
            {
                resource.Links.Add(new LinkDto(update, "update", HttpMethods.Put));
            }

            var delete = _linkGenerator.GetUriByName(HttpContext, "DeleteMoto", new { version = CurrentVersion, id = moto.Id });
            if (!string.IsNullOrWhiteSpace(delete))
            {
                resource.Links.Add(new LinkDto(delete, "delete", HttpMethods.Delete));
            }

            return resource;
        }
    }
}
