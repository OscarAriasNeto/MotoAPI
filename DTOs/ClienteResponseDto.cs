using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Representação de retorno de cliente.
    /// </summary>
    public record ClienteResponseDto(
        int Id,
        string Nome,
        string Email,
        DateTime DataCadastro
    )
    {
        public static ClienteResponseDto FromEntity(Cliente cliente) => new(
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.DataCadastro);
    }
}
