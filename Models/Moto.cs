using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoAPI.Models
{
    public class Moto
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        [Column("ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Modelo é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Modelo deve ter entre {2} e {1} caracteres")]
        [Column("MODELO", TypeName = "VARCHAR2(100)")]
        public string Modelo { get; set; }

        [Range(1900, 2100, ErrorMessage = "Ano de fabricação deve estar entre {1} e {2}")]
        [Column("ANO_FABRICACAO")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "Placa é obrigatória")]
        [StringLength(7)]
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Formato de placa inválido. Use AAA9999")]
        [Column("PLACA", TypeName = "VARCHAR2(7)")]
        public string Placa { get; set; }

        [Column("ESTADO", TypeName = "VARCHAR2(20)")]
        public EstadoMoto Estado { get; set; } = EstadoMoto.Pronta;

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