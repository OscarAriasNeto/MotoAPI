using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Representação de retorno de motocicleta.
    /// </summary>
    public record MotoResponseDto(
        int Id,
        string Modelo,
        int AnoFabricacao,
        string Placa,
        decimal ValorDiaria,
        EstadoMoto Estado,
        string Cor
    )
    {
        public static MotoResponseDto FromEntity(Moto moto) => new(
            moto.Id,
            moto.Modelo,
            moto.AnoFabricacao,
            moto.Placa,
            moto.ValorDiaria,
            moto.Estado,
            moto.Cor);
    }
}
