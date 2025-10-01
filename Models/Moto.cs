using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoAPI.Models
{
    /// <summary>
    /// Representa uma motocicleta disponível para aluguel.
    /// </summary>
    public class Moto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Modelo { get; set; } = string.Empty;

        [Range(1990, 2100)]
        public int AnoFabricacao { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Formato de placa inválido. Utilize AAA9999")]
        [StringLength(7)]
        public string Placa { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal ValorDiaria { get; set; }

        public EstadoMoto Estado { get; set; } = EstadoMoto.Pronta;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public string Cor => Estado switch
        {
            EstadoMoto.Pronta => "Verde",
            EstadoMoto.Lavagem => "Azul",
            EstadoMoto.Sinistro => "Vermelho",
            EstadoMoto.Manutencao => "Amarelo",
            _ => "Desconhecido"
        };
    }
}
