using System.ComponentModel.DataAnnotations;
using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Requisição para atualização de pedido.
    /// </summary>
    public class UpdatePedidoDto
    {
        /// <example>2025-05-26T10:00:00Z</example>
        public DateTime? DataRetirada { get; set; }

        /// <example>2025-05-30T10:00:00Z</example>
        public DateTime? DataDevolucao { get; set; }

        /// <example>520.00</example>
        [Range(0, double.MaxValue)]
        public decimal? ValorTotal { get; set; }

        /// <example>EmAndamento</example>
        public StatusPedido? Status { get; set; }
    }
}
