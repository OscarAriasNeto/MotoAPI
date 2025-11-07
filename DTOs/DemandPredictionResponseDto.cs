namespace MotoAPI.DTOs;

public class DemandPredictionResponseDto
{
    public float DemandaPrevista { get; set; }

    public string Unidade { get; set; } = "reservas";
}
