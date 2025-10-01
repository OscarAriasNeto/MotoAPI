using System.ComponentModel.DataAnnotations;
using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Requisição para criação de pedido.
    /// </summary>
    public class CreatePedidoDto
    {
        /// <example>1</example>
        [Required]
        public int ClienteId { get; set; }

        /// <example>1</example>
        [Required]
        public int MotoId { get; set; }

        /// <example>2025-05-25T10:00:00Z</example>
        public DateTime DataRetirada { get; set; } = DateTime.UtcNow;

        /// <example>2025-05-28T10:00:00Z</example>
        public DateTime? DataDevolucao { get; set; }

        /// <example>450.00</example>
        [Range(0, double.MaxValue)]
        public decimal ValorTotal { get; set; }

        /// <example>Reservado</example>
        public StatusPedido Status { get; set; } = StatusPedido.Reservado;
    }
}
