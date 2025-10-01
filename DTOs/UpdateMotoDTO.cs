using System.ComponentModel.DataAnnotations;
using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Requisição para atualização de motocicleta.
    /// </summary>
    public class UpdateMotoDto
    {
        /// <example>Honda CB 500X</example>
        [StringLength(120)]
        public string? Modelo { get; set; }

        /// <example>2025</example>
        [Range(1990, 2100)]
        public int? AnoFabricacao { get; set; }

        /// <example>DEF5678</example>
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Formato de placa inválido. Utilize AAA9999")]
        public string? Placa { get; set; }

        /// <example>175.0</example>
        [Range(0, double.MaxValue)]
        public decimal? ValorDiaria { get; set; }

        /// <example>Manutencao</example>
        public EstadoMoto? Estado { get; set; }
    }
}
