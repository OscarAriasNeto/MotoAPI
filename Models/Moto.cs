using System.ComponentModel.DataAnnotations;

namespace MotoAPI.Models
{
    public class Moto
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Modelo é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Modelo deve ter entre {2} e {1} caracteres")]
        public string Modelo { get; set; }

        [Range(1900, 2100, ErrorMessage = "Ano de fabricação deve estar entre {1} e {2}")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "Placa é obrigatória")]
        [StringLength(7)]
        [RegularExpression(@"^[A-Z]{3}\d{4}$", ErrorMessage = "Formato de placa inválido. Use AAA9999")]
        public string Placa { get; set; }

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