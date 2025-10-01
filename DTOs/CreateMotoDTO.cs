using System.ComponentModel.DataAnnotations;
using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Requisição para criação de motocicleta.
    /// </summary>
    public class CreateMotoDto
    {
        /// <example>Honda CB 500</example>
        [Required]
        [StringLength(120)]
        public string Modelo { get; set; } = string.Empty;

        /// <example>2024</example>
        [Range(1990, 2100)]
        public int AnoFabricacao { get; set; }

        /// <example>ABC1234</example>
        [Required]
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Formato de placa inválido. Utilize AAA9999")]
        public string Placa { get; set; } = string.Empty;

        /// <example>150.0</example>
        [Range(0, double.MaxValue)]
        public decimal ValorDiaria { get; set; }

        /// <example>Pronta</example>
        public EstadoMoto Estado { get; set; } = EstadoMoto.Pronta;
    }
}
