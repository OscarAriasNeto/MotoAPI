using System.ComponentModel.DataAnnotations;

namespace MotoAPI.DTOs;

public class DemandPredictionRequestDto
{
    [Range(10, 1000, ErrorMessage = "Informe um valor de diária entre 10 e 1000.")]
    public float ValorDiaria { get; set; }

    [Range(1, 31, ErrorMessage = "Informe a média de dias de uso por cliente entre 1 e 31.")]
    public float MediaDiasUsoPorCliente { get; set; }

    [Range(10, 1000, ErrorMessage = "Informe a quilometragem média mensal entre 10 e 1000.")]
    public float QuilometragemMediaMensal { get; set; }
}
