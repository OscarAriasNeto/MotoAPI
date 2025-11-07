using Microsoft.AspNetCore.Mvc;
using MotoAPI.DTOs;
using MotoAPI.Services;

namespace MotoAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/analises")]
[Produces("application/json")]
public class AnalisesController : ControllerBase
{
    private readonly IDemandPredictionService _demandPredictionService;

    public AnalisesController(IDemandPredictionService demandPredictionService)
    {
        _demandPredictionService = demandPredictionService;
    }

    /// <summary>
    /// Realiza previsão de demanda de reservas de motos utilizando ML.NET.
    /// </summary>
    [HttpPost("previsao-demanda", Name = "PredictMotoDemand")]
    [ProducesResponseType(typeof(DemandPredictionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<DemandPredictionResponseDto> PreverDemanda([FromBody] DemandPredictionRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }

        var request = new DemandPredictionRequest(dto.ValorDiaria, dto.MediaDiasUsoPorCliente, dto.QuilometragemMediaMensal);
        var result = _demandPredictionService.Predict(request);

        var response = new DemandPredictionResponseDto
        {
            DemandaPrevista = result.DemandaPrevista
        };

        return Ok(response);
    }
}
