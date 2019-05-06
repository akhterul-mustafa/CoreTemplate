using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Dell.Solution.Service.Sample.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private ILogger _logger;
        
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        [Produces("application/json")]
        public IActionResult HealthCheck()
        {
            try
            {
                var resp = new { ping = DateTime.Now.ToString("F") };
                var response = Ok(resp);
                _logger.LogInformation("HEALTH CHECK: /ping");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: /ping");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
