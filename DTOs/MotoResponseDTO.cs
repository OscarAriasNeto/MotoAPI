using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// DTO para resposta de moto
    /// </summary>
    public record MotoResponseDTO(
        int Id,
        string Modelo,
        int AnoFabricacao,
        string Placa,
        EstadoMoto Estado,
        string Cor)
    {
        public MotoResponseDTO(Moto moto) : this(
            moto.Id,
            moto.Modelo,
            moto.AnoFabricacao,
            moto.Placa,
            moto.Estado,
            moto.Cor)
        { }
    }
}