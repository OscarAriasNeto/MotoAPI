using System.ComponentModel.DataAnnotations;
using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// DTO para criação de nova moto
    /// </summary>
    public record CreateMotoDTO(
        [Required(ErrorMessage = "ID é obrigatório")]
        int Id,

        [Required(ErrorMessage = "Modelo é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Modelo deve ter entre {2} e {1} caracteres")]
        string Modelo,

        [Range(1900, 2100, ErrorMessage = "Ano de fabricação deve estar entre {1} e {2}")]
        int AnoFabricacao,

        [Required(ErrorMessage = "Placa é obrigatória")]
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Formato de placa inválido. Use AAA9999")]
        string Placa,

        EstadoMoto Estado = EstadoMoto.Pronta);
}