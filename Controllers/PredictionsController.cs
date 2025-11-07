using Microsoft.AspNetCore.Mvc;
using MotoAPI.Services;

namespace MotoAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/predicoes")]
    [Produces("application/json")]
    public class PredictionsController : ControllerBase
    {
        private readonly IMotoPricePredictionService _predictionService;

        public PredictionsController(IMotoPricePredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        /// <summary>
        /// Realiza uma previsão simples de valor de diária para uma moto.
        /// </summary>
        [HttpPost("motos/valor-diaria")]
        [ProducesResponseType(typeof(MotoPricePrediction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<MotoPricePrediction> PredictMotoDailyRate([FromBody] PredictMotoPriceRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var prediction = _predictionService.Predict(request);
            return Ok(prediction);
        }
    }
}
