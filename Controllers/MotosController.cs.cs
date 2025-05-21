using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotoAPI.Models;
using MotoAPI.DTOs;
using MotoAPI.Data;

namespace MotoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MotosController : ControllerBase
    {
        private readonly MotoDbContext _context;

        public MotosController(MotoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca moto por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<MotoResponseDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetPorId(int id)
        {
            var moto = await _context.Motos.FindAsync(id);

            return moto == null
                ? NotFound(new ApiResponse<object>(false, $"Moto ID {id} não encontrada"))
                : Ok(new ApiResponse<MotoResponseDTO>(true, "Moto encontrada", new MotoResponseDTO(moto)));
        }

        /// <summary>
        /// Busca moto por placa
        /// </summary>
        [HttpGet("placa/{placa}")]
        [ProducesResponseType(typeof(ApiResponse<MotoResponseDTO>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetPorPlaca(string placa)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);

            return moto == null
                ? NotFound(new ApiResponse<object>(false, $"Placa {placa} não encontrada"))
                : Ok(new ApiResponse<MotoResponseDTO>(true, "Moto encontrada", new MotoResponseDTO(moto)));
        }

        /// <summary>
        /// Cadastra nova moto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MotoResponseDTO>), 201)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Post([FromBody] CreateMotoDTO dto)
        {
            if (await _context.Motos.AnyAsync(m => m.Id == dto.Id))
                return Conflict(new ApiResponse<object>(false, $"ID {dto.Id} já cadastrado"));

            if (await _context.Motos.AnyAsync(m => m.Placa == dto.Placa))
                return Conflict(new ApiResponse<object>(false, $"Placa {dto.Placa} já cadastrada"));

            var moto = new Moto
            {
                Id = dto.Id,
                Modelo = dto.Modelo,
                AnoFabricacao = dto.AnoFabricacao,
                Placa = dto.Placa,
                Estado = dto.Estado
            };

            _context.Motos.Add(moto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorId),
                new { id = moto.Id },
                new ApiResponse<MotoResponseDTO>(true, "Moto cadastrada com sucesso", new MotoResponseDTO(moto)));
        }

        /// <summary>
        /// Atualiza moto por ID
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateMotoDTO dto)
        {
            var moto = await _context.Motos.FindAsync(id);
            if (moto == null)
                return NotFound(new ApiResponse<object>(false, $"Moto ID {id} não encontrada"));

            return await AtualizarMoto(moto, dto);
        }

        /// <summary>
        /// Atualiza moto por placa
        /// </summary>
        [HttpPut("placa/{placa}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        [ProducesResponseType(typeof(ApiResponse<object>), 409)]
        public async Task<IActionResult> PutPorPlaca(string placa, [FromBody] UpdateMotoDTO dto)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
            if (moto == null)
                return NotFound(new ApiResponse<object>(false, $"Placa {placa} não encontrada"));

            return await AtualizarMoto(moto, dto);
        }

        /// <summary>
        /// Remove moto por ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Delete(int id)
        {
            var moto = await _context.Motos.FindAsync(id);
            return moto == null
                ? NotFound(new ApiResponse<object>(false, $"Moto ID {id} não encontrada"))
                : await RemoverMoto(moto);
        }

        /// <summary>
        /// Remove moto por placa
        /// </summary>
        [HttpDelete("placa/{placa}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> DeletePorPlaca(string placa)
        {
            var moto = await _context.Motos.FirstOrDefaultAsync(m => m.Placa == placa);
            return moto == null
                ? NotFound(new ApiResponse<object>(false, $"Placa {placa} não encontrada"))
                : await RemoverMoto(moto);
        }

        private async Task<IActionResult> AtualizarMoto(Moto moto, UpdateMotoDTO dto)
        {
            // Validação de placa duplicada
            if (!string.IsNullOrEmpty(dto.Placa) && dto.Placa != moto.Placa)
            {
                if (await _context.Motos.AnyAsync(m => m.Placa == dto.Placa))
                    return Conflict(new ApiResponse<object>(false, "Nova placa já está em uso"));
            }

            // Aplicar atualizações
            moto.Modelo = dto.Modelo ?? moto.Modelo;
            moto.AnoFabricacao = dto.AnoFabricacao ?? moto.AnoFabricacao;
            moto.Placa = dto.Placa ?? moto.Placa;
            moto.Estado = dto.Estado ?? moto.Estado;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Motos.AnyAsync(m => m.Id == moto.Id))
                    return NotFound(new ApiResponse<object>(false, "Moto não encontrada durante atualização"));
                throw;
            }
        }

        private async Task<IActionResult> RemoverMoto(Moto moto)
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}