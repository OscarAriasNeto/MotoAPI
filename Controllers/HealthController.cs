using Microsoft.AspNetCore.Mvc;

namespace MotoAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/health")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Retorna o status de saúde da aplicação.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get() => Ok(new { status = "Healthy" });
    }
}
