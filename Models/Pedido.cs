using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoAPI.Models
{
    /// <summary>
    /// Representa um pedido de locação de motocicleta.
    /// </summary>
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        [Required]
        public int MotoId { get; set; }

        public Moto? Moto { get; set; }

        public DateTime DataRetirada { get; set; } = DateTime.UtcNow;

        public DateTime? DataDevolucao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal ValorTotal { get; set; }

        public StatusPedido Status { get; set; } = StatusPedido.Reservado;
    }
}
