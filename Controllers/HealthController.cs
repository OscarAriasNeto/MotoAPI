using Microsoft.AspNetCore.Mvc;

namespace MotoAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/health")]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get() => Ok(new { status = "Healthy" });
    }
}
